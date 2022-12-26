using EPGP.Data.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace EPGP.Data.DbContexts
{
    public class RaiderContext : SharedDbContext
    {
        public DbSet<Raider> Raiders { get; set; }

        public RaiderContext()
        {
            Database.EnsureCreated();
        }
    }

    public class Raider
    {
        [Key]
        public int RaiderId { get; set; }

        public string CharacterName { get; init; }

        public Region Region { get; set; }

        public string Realm { get; init; }

        public Class Class { get; set; }

        public virtual EffortPoints EffortPoints { get; set; }

        public virtual GearPoints GearPoints { get; set; }
    }
}
