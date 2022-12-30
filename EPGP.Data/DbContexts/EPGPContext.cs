using EPGP.Data.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace EPGP.Data.DbContexts;
public class EPGPContext : DbContext
{
    public EPGPContext(DbContextOptions<EPGPContext> options) : base(options) { }

    public DbSet<Raider> Raiders { get; set; }

    public DbSet<EffortPoints> EffortPoints { get; set; }

    public DbSet<GearPoints> GearPoints { get; set; }

    public DbSet<LootHistoryMatch> LootHistoryMatch { get; set; }

    public DbSet<LootHistoryGearPoints> LootHistoryGearPoints { get; set; }

    public DbSet<LootHistoryDetailed> LootHistoryDetailed { get; set; }
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

    public virtual IEnumerable<LootHistoryMatch> LootHistory { get; set; }
}

public class EffortPoints
{
    [Key]
    public int EffortPointsId { get; set; }

    public int RaiderId { get; set; }

    public decimal Points { get; set; }

    public virtual Raider Raider { get; set; }
}

public class GearPoints
{
    [Key]
    public int GearPointsId { get; set; }

    public int RaiderId { get; set; }

    public decimal Points { get; set; }

    public virtual Raider Raider { get; set; }
}

public class LootHistoryDetailed
{
    public int LootHistoryDetailedId { get; set; }

    public string ItemString { get; set; }

    public string? Instance { get; set; }

    public string? Boss { get; set; }
}

public class LootHistoryGearPoints
{
    public int LootHistoryGearPointsId { get; set; }

    public string ItemString { get; set; }

    public decimal GearPoints { get; set; }
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
