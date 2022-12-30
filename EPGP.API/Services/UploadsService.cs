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
        private readonly IBackgroundJobClient _backgroundJobs;
        private readonly ILootHistoryRepository _lootHistoryRepo;

        public UploadsService(IRaiderService raiderService, IPointsService pointsService, IBackgroundJobClient backgroundJobs, ILootHistoryRepository lootHistoryRepo) =>
            (_raiderService, _pointsService, _backgroundJobs, _lootHistoryRepo) = (raiderService, pointsService, backgroundJobs, lootHistoryRepo);

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

        public void ProcessRoster(Region region, IEnumerable<UploadEPGPRoster> roster)
        {
            foreach (var raider in roster)
            {
                var raiderId = _raiderService.CreateRaider(new Models.Raider
                {
                    CharacterName = raider.CharacterName,
                    Realm = raider.Realm,
                    Region = region,
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
                    el.LootHistoryGearPoints.ItemString == uploadedLoot.ItemString &&
                    el.LootHistoryGearPoints.GearPoints == uploadedLoot.GearPoints
                ))
                {
                    continue;
                }

                var raider = _raiderService.GetRaider(uploadedLoot.CharacterName, uploadedLoot.Realm);

                var lootHistoryGearPointsId = _lootHistoryRepo.AddLootHistoryGearPoints(new LootHistoryGearPoints
                {
                    ItemString = uploadedLoot.ItemString,
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
    }
}
