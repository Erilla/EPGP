using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace EPGP.Data.DbContexts
{
    public class LootHistoryContext : SharedDbContext
    {
        public DbSet<LootHistory> LootHistory { get; set; }

        public LootHistoryContext() : base() { }
    }

    public class LootHistory
    {
        [Key]
        public int LootHistoryId { get; set; }

        public int RaiderId { get; set; }

        public DateTime Date { get; set; }

        public string ItemString { get; set; }

        public string? Instance { get; set; }

        public string? Boss { get; set; }

        public string? GearPoints { get; set; }

        public virtual Raider Raider { get; set; }
    }
}
