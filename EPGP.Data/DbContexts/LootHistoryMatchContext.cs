using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace EPGP.Data.DbContexts
{
    public class LootHistoryMatchContext : SharedDbContext
    {
        public DbSet<LootHistoryMatch> LootHistoryMatch { get; set; }

        public LootHistoryMatchContext() : base() { }
    }

    public class LootHistoryMatch
    {
        [Key]
        public int LootHistoryMatchId { get; set; }

        public int RaiderId { get; set; }

        public DateTime Date { get; set; }

        public int? LootHistoryGearPointsId { get; set; }

        public int? LootHistoryDetailedId { get; set; }

        public virtual Raider Raider { get; set; }

        public virtual LootHistoryGearPoints LootHistoryGearPoints { get; set; }

        public virtual LootHistoryDetailed LootHistoryDetailed { get; set; }

    }
}
