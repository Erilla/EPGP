using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace EPGP.Data.DbContexts
{
    public class EffortPointsContext : SharedDbContext
    {
        public DbSet<EffortPoints> EffortPoints { get; set; }

        public EffortPointsContext() : base()
        {
        }
    }

    public class EffortPoints
    {
        [Key]
        public int EffortPointsId { get; set; }

        public int RaiderId { get; set; }

        public decimal Points { get; set; }

        public virtual Raider Raider { get; set; }
    }
}
