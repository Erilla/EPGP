using EPGP.Data.DbContexts;

namespace EPGP.Data.Repositories
{
    public interface ILootHistoryRepository
    {
        IEnumerable<LootHistoryMatch> GetLootHistoryForRaider(int raiderId);

        void AddLootHistoryMatch(LootHistoryMatch lootHistoryMatch);

        void UpdateLootHistoryMatch(LootHistoryMatch lootHistoryMatch);

        void AddLootHistoryGearPoints(LootHistoryGearPoints lootHistoryGearPoints);

        void UpdateLootHistoryGearPoints(LootHistoryGearPoints lootHistoryGearPoints);

        void AddLootHistoryDetailed(LootHistoryDetailed lootHistoryDetailed);

        void UpdateLootHistoryDetailed(LootHistoryDetailed lootHistoryDetailed);
    }
}
