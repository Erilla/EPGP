using EPGP.Data.DbContexts;

namespace EPGP.Data.Repositories
{
    public class LootHistoryRepository : ILootHistoryRepository
    {
        public void AddLootHistory(LootHistory lootHistory)
        {
            using var context = new LootHistoryContext();
            context.LootHistory.Add(lootHistory);
            context.SaveChanges();
        }

        public IEnumerable<LootHistory> GetLootHistoryForRaider(int raiderId)
        {
            using var context = new LootHistoryContext();
            return context.LootHistory.Where(lh => lh.RaiderId == raiderId).ToList();
        }

        public void UpdateLootHistory(LootHistory lootHistory)
        {
            using var context = new LootHistoryContext();
            context.LootHistory.Update(lootHistory);
            context.SaveChanges();
        }
    }
}
