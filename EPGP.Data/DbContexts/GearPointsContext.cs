using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace EPGP.Data.DbContexts;

public class GearPointsContext : SharedDbContext
{
    public DbSet<GearPoints> GearPoints { get; set; }

    public GearPointsContext() : base()
    {
    }
}

public class GearPoints
{
    [Key]
    public int GearPointsId { get; set; }

    public int RaiderId { get; set; }

    public decimal Points { get; set; }

    public virtual Raider Raider { get; set; }
}
