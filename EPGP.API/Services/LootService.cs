using AutoMapper;
using EPGP.API.Models;
using EPGP.API.Responses;
using EPGP.Data.DbContexts;
using EPGP.Data.Enums;
using EPGP.Data.Repositories;

namespace EPGP.API.Services
{
    public class LootService : ILootService
    {
        private readonly ILootHistoryRepository _lootHistoryRepository;
        private readonly IMapper _mapper;

        public LootService(ILootHistoryRepository lootHistoryRepository, IMapper mapper) => (_lootHistoryRepository, _mapper) = (lootHistoryRepository, mapper);

        public int AddItemString(Data.DbContexts.ItemString itemString) => _lootHistoryRepository.AddItemString(itemString);

        public void AddItemStringAdditionalIds(ICollection<ItemStringAdditionalIds> itemStringAdditionalIds)
            => _lootHistoryRepository.AddItemStringAdditionalIds(itemStringAdditionalIds);

        public void AddModifiers(ICollection<Data.DbContexts.Modifier> modifiers) => _lootHistoryRepository.AddModifiers(modifiers);

        public LootHistory GetLootHistory(int raiderId, int pageSize)
        {
            var (lootHistoryResult, totalLoots) = _lootHistoryRepository.GetPagedLootHistoryForRaider(raiderId, pageSize);

            return new LootHistory
            {
                RaiderId = raiderId,
                TotalNumberOfLoots = totalLoots,
                Loots = lootHistoryResult.Select(lhr => new Loot
                {
                    LootHistoryId = lhr.LootHistoryMatchId,
                    Timestamp = lhr.Date,
                    ItemString = _mapper.Map<Models.ItemString>(lhr.LootHistoryGearPoints.ItemString),
                    GearPoints = lhr.LootHistoryGearPoints.GearPoints
                })
            };
        }

        public LootHistory GetLootHistory(Region region, string realm, string characterName, int pageSize)
        {
            var (lootHistoryResult, totalLoots, raiderId) = _lootHistoryRepository.GetPagedLootHistoryForRaider(region, realm, characterName, pageSize);

            return new LootHistory
            {
                RaiderId = raiderId,
                TotalNumberOfLoots = totalLoots,
                Loots = lootHistoryResult.Select(lhr => new Loot
                {
                    LootHistoryId = lhr.LootHistoryMatchId,
                    Timestamp = lhr.Date,
                    ItemString = _mapper.Map<Models.ItemString>(lhr.LootHistoryGearPoints.ItemString),
                    GearPoints = lhr.LootHistoryGearPoints.GearPoints
                })
            };
        }

        public LootHistoryByDateResponse GetLootHistoryByDate(int page)
        {
            var (raidDates, raidDate, loot) = _lootHistoryRepository.GetPagedLootHistoryByDate(page);

            return new LootHistoryByDateResponse
            {
                RaidDates = raidDates,
                LootHistory = new LootHistoryByDate
                {
                    RaidDate = raidDate,
                    Loot = loot.Select(l => new LootByDate
                    {
                        ItemString = _mapper.Map<Models.ItemString>(l.LootHistoryGearPoints.ItemString),
                        CharacterName = l.Raider.CharacterName,
                        CharacterClass = l.Raider.Class,
                        Realm = l.Raider.Realm,
                        Region = l.Raider.Region,
                        GearPoints = l.LootHistoryGearPoints.GearPoints
                    })
                }
            };
        }
    }
}
