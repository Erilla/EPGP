using Microsoft.EntityFrameworkCore;

namespace EPGP.Data.DbContexts
{
    public class LootHistoryDetailedContext : SharedDbContext
    {
        public DbSet<LootHistoryDetailed> LootHistoryDetailed { get; set; }

        public LootHistoryDetailedContext() : base() { }
    }

    public class LootHistoryDetailed
    {
        public int LootHistoryDetailedId { get; set; }

        public int LootHistoryMatchId { get; set; }

        public string ItemString { get; set; }

        public string? Instance { get; set; }

        public string? Boss { get; set; }
    }
}
