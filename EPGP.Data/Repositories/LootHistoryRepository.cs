using EPGP.Data.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace EPGP.Data.Repositories
{
    public class LootHistoryRepository : ILootHistoryRepository
    {
        public IEnumerable<LootHistoryMatch> GetLootHistoryForRaider(int raiderId)
        {
            using var context = new LootHistoryMatchContext();
            return context.LootHistoryMatch
                .Where(lh => lh.RaiderId == raiderId)
                .Include(lh => lh.LootHistoryGearPoints)
                .Include(lh => lh.LootHistoryDetailed)
                .ToList();
        }

        public void AddLootHistoryMatch(LootHistoryMatch lootHistoryMatch)
        {
            using var context = new LootHistoryMatchContext();
            context.LootHistoryMatch.Add(lootHistoryMatch);
            context.SaveChanges();
        }

        public void UpdateLootHistoryMatch(LootHistoryMatch lootHistoryMatch)
        {
            using var context = new LootHistoryMatchContext();
            context.LootHistoryMatch.Update(lootHistoryMatch);
            context.SaveChanges();
        }

        public void AddLootHistoryGearPoints(LootHistoryGearPoints lootHistoryGearPoints)
        {
            using var context = new LootHistoryGearPointsContext();
            context.LootHistoryGearPoints.Add(lootHistoryGearPoints);
            context.SaveChanges();
        }

        public void UpdateLootHistoryGearPoints(LootHistoryGearPoints lootHistoryGearPoints)
        {
            using var context = new LootHistoryGearPointsContext();
            context.LootHistoryGearPoints.Update(lootHistoryGearPoints);
            context.SaveChanges();
        }

        public void AddLootHistoryDetailed(LootHistoryDetailed lootHistoryDetailed)
        {
            using var context = new LootHistoryDetailedContext();
            context.LootHistoryDetailed.Add(lootHistoryDetailed);
            context.SaveChanges();
        }

        public void UpdateLootHistoryDetailed(LootHistoryDetailed lootHistoryDetailed)
        {
            using var context = new LootHistoryDetailedContext();
            context.LootHistoryDetailed.Update(lootHistoryDetailed);
            context.SaveChanges();
        }
    }
}
