using Microsoft.EntityFrameworkCore;

namespace EPGP.Data.DbContexts
{
    public class LootHistoryGearPointsContext : SharedDbContext
    {
        public DbSet<LootHistoryGearPoints> LootHistoryGearPoints { get; set; }

        public LootHistoryGearPointsContext() : base() { }
    }

    public class LootHistoryGearPoints
    {
        public int LootHistoryGearPointsId { get; set; }

        public int LootHistoryMatchId { get; set; }

        public string ItemString { get; set; }

        public string? GearPoints { get; set; }
    }
}
