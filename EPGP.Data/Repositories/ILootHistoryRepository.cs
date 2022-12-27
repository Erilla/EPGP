using EPGP.Data.DbContexts;

namespace EPGP.Data.Repositories
{
    public interface ILootHistoryRepository
    {
        IEnumerable<LootHistory> GetLootHistoryForRaider(int raiderId);

        void AddLootHistory(LootHistory lootHistory);

        void UpdateLootHistory(LootHistory lootHistory);
    }
}
