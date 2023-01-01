using EPGP.API.Requests;
using EPGP.API.Responses;
using EPGP.Data.DbContexts;
using EPGP.Data.Enums;
using EPGP.Data.Repositories;
using Hangfire;

namespace EPGP.API.Services
{
    public class UploadsService : IUploadsService
    {
        private readonly IRaiderService _raiderService;
        private readonly IPointsService _pointsService;
        private readonly ILootService _lootService;
        private readonly IBackgroundJobClient _backgroundJobs;
        private readonly ILootHistoryRepository _lootHistoryRepo;
        private readonly IRaiderIoService _raiderIoService;


        public UploadsService(IRaiderService raiderService, IPointsService pointsService, IBackgroundJobClient backgroundJobs, ILootHistoryRepository lootHistoryRepo, IRaiderIoService raiderIoService, ILootService lootService) =>
            (_raiderService, _pointsService, _backgroundJobs, _lootHistoryRepo, _raiderIoService, _lootService) = (raiderService, pointsService, backgroundJobs, lootHistoryRepo, raiderIoService, lootService);

        public UploadEPGPResponse ProcessEPGP(UploadEPGPRequest request)
        {
            var region = string.IsNullOrEmpty(request.Region) ? Region.Unknown : (Region)Enum.Parse(typeof(Region), request.Region);

            var rosterJobId = _backgroundJobs.Enqueue(() => ProcessRoster(region, ConvertRosterObjectToClass(request.Roster)));

            var lootJobIds = new List<string>();

            foreach (var lootByDate in ConvertLootObjectToClass(request.Loot))
            {
                lootJobIds.Add(_backgroundJobs
                    .ContinueJobWith(
                        rosterJobId,
                        () => ProcessLoot(lootByDate.Key, lootByDate.ToList())
                    ));
            }

            return new UploadEPGPResponse
            {
                RosterJobId = rosterJobId,
                LootJobIds = lootJobIds
            };
        }

        public async Task ProcessRoster(Region region, IEnumerable<UploadEPGPRoster> roster)
        {
            foreach (var raider in roster)
            {
                var characterProfile = await _raiderIoService.GetCharactersProfile(region, raider.Realm, raider.CharacterName);

                var raiderId = _raiderService.CreateRaider(new Models.Raider
                {
                    CharacterName = raider.CharacterName,
                    Realm = raider.Realm,
                    Region = region,
                    Class = (Class)Enum.Parse(typeof(Class), characterProfile.Class, true)
                });

                _pointsService.UpdateEffortPoints(raiderId, raider.EffortPoints);
                _pointsService.UpdateGearPoints(raiderId, raider.GearPoints);
            }
        }

        public void ProcessLoot(DateOnly date, IEnumerable<UploadEPGPLoot> uploadEPGPLoots)
        {
            var existingLoot = _lootHistoryRepo.GetLootHistoryMatchByDate(date);

            foreach (var uploadedLoot in uploadEPGPLoots)
            {
                if (existingLoot.Any(el =>
                    el.Raider.CharacterName == uploadedLoot.CharacterName &&
                    el.LootHistoryGearPoints.ItemString == ConvertItemStringToObject(uploadedLoot.ItemString) &&
                    el.LootHistoryGearPoints.GearPoints == uploadedLoot.GearPoints
                ))
                {
                    continue;
                }

                var raider = _raiderService.GetRaider(uploadedLoot.CharacterName, uploadedLoot.Realm);

                var lootHistoryGearPointsId = _lootHistoryRepo.AddLootHistoryGearPoints(new LootHistoryGearPoints
                {
                    ItemStringId = ConvertItemStringToObject(uploadedLoot.ItemString).ItemStringId,
                    GearPoints = uploadedLoot.GearPoints
                });

                _lootHistoryRepo.AddLootHistoryMatch(new LootHistoryMatch
                {
                    RaiderId = raider.RaiderId,
                    Date = uploadedLoot.Timestamp.ToUniversalTime(),
                    LootHistoryGearPointsId = lootHistoryGearPointsId
                });
            }
        }

        private IEnumerable<UploadEPGPRoster> ConvertRosterObjectToClass(object[][] roster)
        {
            var classObject = new List<UploadEPGPRoster>();

            foreach (var raider in roster)
            {
                var (characterName, characterRealm) = ExtractCharacterString(raider[0].ToString() ?? string.Empty);

                if (string.IsNullOrEmpty(characterName)) continue;

                var effortPointsStringObject = raider[1].ToString() ?? string.Empty;
                var gearPointsStringObject = raider[2].ToString() ?? string.Empty;

                var effortPoints = string.IsNullOrEmpty(effortPointsStringObject) ? 0 : int.Parse(effortPointsStringObject);
                var gearPoints = string.IsNullOrEmpty(gearPointsStringObject) ? 0 : int.Parse(gearPointsStringObject);

                classObject.Add(new UploadEPGPRoster
                {
                    CharacterName = characterName,
                    Realm = characterRealm,
                    EffortPoints = effortPoints,
                    GearPoints = gearPoints
                });
            }

            return classObject;
        }

        private IEnumerable<IGrouping<DateOnly, UploadEPGPLoot>> ConvertLootObjectToClass(object[][] loots)
        {
            var classObject = new List<UploadEPGPLoot>();

            foreach (var loot in loots)
            {
                var timeStamp = loot[0].ToString() ?? string.Empty;
                var timeStampDateTime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(timeStamp));

                var (characterName, characterRealm) = ExtractCharacterString(loot[1].ToString() ?? string.Empty);

                if (string.IsNullOrEmpty(characterName)) continue;

                var itemString = loot[2].ToString() ?? string.Empty;

                var gearPointsStringObject = loot[3].ToString() ?? string.Empty;
                var gearPoints = string.IsNullOrEmpty(gearPointsStringObject) ? 0 : int.Parse(gearPointsStringObject);

                classObject.Add(new UploadEPGPLoot
                {
                    Timestamp = timeStampDateTime.DateTime,
                    CharacterName = characterName,
                    Realm = characterRealm,
                    ItemString = itemString,
                    GearPoints = gearPoints
                });
            }

            return classObject.GroupBy(l => DateOnly.FromDateTime(l.Timestamp)).ToList();
        }

        private (string, string) ExtractCharacterString(string character)
        {
            if (string.IsNullOrEmpty(character)) return (string.Empty, string.Empty);

            var characterSplit = character.Split("-");
            return (characterSplit[0], characterSplit[1]);
        }


        private ItemString ConvertItemStringToObject(string itemString)
        {
            var itemStringSplit = itemString.Split(":");

            var result = new ItemString();

            var bonusIdDone = false;
            var modifierDone = false;
            var relic1BonusDone = false;
            var relic2BonusDone = false;
            var relic3BonusDone = false;

            for (int i = 1; i < itemStringSplit.Length; i++)
            {
                var value = itemStringSplit[i];

                if (string.IsNullOrEmpty(value)) continue;

                switch (i)
                {
                    case 1: result.ItemId = value; break;
                    case 2: result.EnchantId = value; break;
                    case 3: result.GemId1 = value; break;
                    case 4: result.GemId2 = value; break;
                    case 5: result.GemId3 = value; break;
                    case 6: result.GemId4 = value; break;
                    case 7: result.SuffixId = value; break;
                    case 8: result.UniqueId = value; break;
                    case 9: result.LinkLevel = value; break;
                    case 10: result.SpecialisationId = value; break;
                    case 11: result.ModifiersMask = value; break;
                    case 12: result.ItemContext = value; break;
                    default:
                        if (!bonusIdDone)
                        {
                            (result.BonusIds, i, bonusIdDone) = RetrieveDynamicIds(value, itemStringSplit, i, AdditionalIdType.BonusId);
                            break;
                        }
                        if (!modifierDone)
                        {
                            int numberOfIds = ParseNumberOfIds(value);

                            var modifiers = new List<Modifier>();

                            for (int j = 1; j < numberOfIds * 2; j += 2)
                            {
                                modifiers.Add(new Modifier
                                {
                                    ModifierType = itemStringSplit[i + j],
                                    ModifierValue = itemStringSplit[i + j + 1],
                                });
                            }

                            _lootService.AddModifiers(modifiers);

                            result.Modifiers = modifiers;
                            modifierDone = true;
                            i += numberOfIds * 2;
                            break;
                        }
                        if (!relic1BonusDone)
                        {
                            (result.Relic1BonusIds, i, relic1BonusDone) = RetrieveDynamicIds(value, itemStringSplit, i, AdditionalIdType.Relic1BonusId);
                            break;
                        }
                        if (!relic2BonusDone)
                        {
                            (result.Relic2BonusIds, i, relic2BonusDone) = RetrieveDynamicIds(value, itemStringSplit, i, AdditionalIdType.Relic2BonusId);
                            break;
                        }
                        if (!relic3BonusDone)
                        {
                            (result.Relic3BonusIds, i, relic3BonusDone) = RetrieveDynamicIds(value, itemStringSplit, i, AdditionalIdType.Relic3BonusId);
                            break;
                        }
                        break;
                }
            }

            var id = _lootService.AddItemString(result);
            result.ItemStringId = id;
            return result;
        }

        private static int ParseNumberOfIds(string value)
        {
            if (!int.TryParse(value, out int numberOfBonusIds))
            {
                numberOfBonusIds = 0;
            }
            return numberOfBonusIds;
        }

        private (ICollection<ItemStringAdditionalIds>, int, bool) RetrieveDynamicIds(string value, string[] itemStringSplit, int currentIndex, AdditionalIdType type)
        {
            int numberOfIds = ParseNumberOfIds(value);

            var result = new List<ItemStringAdditionalIds>();

            for (int i = 1; i < numberOfIds; i++)
            {
                var additionalId = itemStringSplit[currentIndex + i];
                result.Add(new ItemStringAdditionalIds
                {
                    AdditionalId = additionalId,
                    Type = type
                });
            }

            _lootService.AddItemStringAdditionalIds(result);

            return (result, currentIndex + numberOfIds, true);
        }
    }
}
