using EPGP.API.Models;
using EPGP.Data.Repositories;

namespace EPGP.API.Services
{
    public class LootService : ILootService
    {
        private readonly ILootHistoryRepository _lootHistoryRepository;

        public LootService(ILootHistoryRepository lootHistoryRepository) => (_lootHistoryRepository) = lootHistoryRepository;

        public LootHistory GetLootHistory(int raiderId, int pageSize)
        {
            var (lootHistoryResult, totalLoots) = _lootHistoryRepository.GetPagedLootHistoryForRaider(raiderId, pageSize);

            return new LootHistory
            {
                RaiderId = raiderId,
                TotalNumberOfLoots = totalLoots,
                Loots = lootHistoryResult.Select(lhr => new Loot
                {
                    Timestamp = lhr.Date,
                    ItemString = lhr.LootHistoryGearPoints.ItemString,
                    GearPoints = lhr.LootHistoryGearPoints.GearPoints
                })
            };
        }
    }
}
