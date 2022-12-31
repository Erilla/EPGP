using EPGP.Data.DbContexts;

namespace EPGP.Data.Repositories
{
    public interface ILootHistoryRepository
    {
        IEnumerable<LootHistoryMatch> GetLootHistoryForRaider(int raiderId);

        (IEnumerable<LootHistoryMatch>, int) GetPagedLootHistoryForRaider(int raiderId, int pageSize);

        IEnumerable<LootHistoryMatch> GetLootHistoryMatchByDate(DateOnly date);

        int AddLootHistoryMatch(LootHistoryMatch lootHistoryMatch);

        void UpdateLootHistoryMatch(LootHistoryMatch lootHistoryMatch);

        int AddLootHistoryGearPoints(LootHistoryGearPoints lootHistoryGearPoints);

        void UpdateLootHistoryGearPoints(LootHistoryGearPoints lootHistoryGearPoints);

        int AddLootHistoryDetailed(LootHistoryDetailed lootHistoryDetailed);

        void UpdateLootHistoryDetailed(LootHistoryDetailed lootHistoryDetailed);
    }
}
