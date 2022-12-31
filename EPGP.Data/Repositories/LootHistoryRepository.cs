using EPGP.Data.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace EPGP.Data.Repositories
{
    public class LootHistoryRepository : ILootHistoryRepository
    {
        private readonly EPGPContext _epgpContext;

        public LootHistoryRepository(EPGPContext epgpContext) => (_epgpContext) = epgpContext;

        public IEnumerable<LootHistoryMatch> GetLootHistoryForRaider(int raiderId)
        {
            return _epgpContext.LootHistoryMatch
                .Where(lh => lh.RaiderId == raiderId)
                .Include(lh => lh.LootHistoryGearPoints)
                .Include(lh => lh.LootHistoryDetailed)
                .ToList();
        }

        public (IEnumerable<LootHistoryMatch>, int) GetPagedLootHistoryForRaider(int raiderId, int pageSize)
        {
            var result = _epgpContext.LootHistoryMatch
                .Where(lh => lh.RaiderId == raiderId)
                .OrderBy(lh => lh.Date)

                .Include(lh => lh.LootHistoryGearPoints)
                .Include(lh => lh.LootHistoryDetailed)
                .ToList();

            return (result.Take(pageSize), result.Count);
        }

        public int AddLootHistoryMatch(LootHistoryMatch lootHistoryMatch)
        {
            _epgpContext.LootHistoryMatch.Add(lootHistoryMatch);
            _epgpContext.SaveChanges();
            return lootHistoryMatch.LootHistoryMatchId;
        }

        public void UpdateLootHistoryMatch(LootHistoryMatch lootHistoryMatch)
        {
            _epgpContext.LootHistoryMatch.Update(lootHistoryMatch);
            _epgpContext.SaveChanges();
        }

        public int AddLootHistoryGearPoints(LootHistoryGearPoints lootHistoryGearPoints)
        {
            _epgpContext.LootHistoryGearPoints.Add(lootHistoryGearPoints);
            _epgpContext.SaveChanges();
            return lootHistoryGearPoints.LootHistoryGearPointsId;
        }

        public void UpdateLootHistoryGearPoints(LootHistoryGearPoints lootHistoryGearPoints)
        {
            _epgpContext.LootHistoryGearPoints.Update(lootHistoryGearPoints);
            _epgpContext.SaveChanges();
        }

        public int AddLootHistoryDetailed(LootHistoryDetailed lootHistoryDetailed)
        {
            _epgpContext.LootHistoryDetailed.Add(lootHistoryDetailed);
            _epgpContext.SaveChanges();
            return lootHistoryDetailed.LootHistoryDetailedId;
        }

        public void UpdateLootHistoryDetailed(LootHistoryDetailed lootHistoryDetailed)
        {
            _epgpContext.LootHistoryDetailed.Update(lootHistoryDetailed);
            _epgpContext.SaveChanges();
        }

        public IEnumerable<LootHistoryMatch> GetLootHistoryMatchByDate(DateOnly date)
        {
            return _epgpContext.LootHistoryMatch
                .Where(lhm => DateOnly.FromDateTime(lhm.Date) == date)
                .Include(lhm => lhm.Raider)
                .Include(lhm => lhm.LootHistoryGearPoints)
                .Include(lhm => lhm.LootHistoryDetailed)
                .ToList();
        }
    }
}
