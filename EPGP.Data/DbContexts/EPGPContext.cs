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

    public DbSet<ItemString> ItemStrings { get; set; }

    public DbSet<ItemStringAdditionalIds> ItemStringAdditionalIds { get; set; }

    public DbSet<Modifier> Modifiers { get; set; }

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

    public string ItemStringId { get; set; }

    public string? Instance { get; set; }

    public string? Boss { get; set; }

    public virtual ItemString ItemString { get; set; }
}

public class LootHistoryGearPoints
{
    public int LootHistoryGearPointsId { get; set; }

    public int ItemStringId { get; set; }

    public decimal GearPoints { get; set; }

    public virtual ItemString ItemString { get; set; }
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

public class ItemString
{
    [Key]
    public int ItemStringId { get; set; }

    public string? ItemId { get; set; }

    public string? EnchantId { get; set; }

    public string? GemId1 { get; set; }

    public string? GemId2 { get; set; }

    public string? GemId3 { get; set; }

    public string? GemId4 { get; set; }

    public string? SuffixId { get; set; }

    public string? UniqueId { get; set; }

    public string? LinkLevel { get; set; }

    public string? SpecialisationId { get; set; }

    public string? ModifiersMask { get; set; }

    public string? ItemContext { get; set; }

    public ICollection<ItemStringAdditionalIds> BonusIds { get; set; }

    public ICollection<Modifier> Modifiers { get; set; }

    public ICollection<ItemStringAdditionalIds> Relic1BonusIds { get; set; }

    public ICollection<ItemStringAdditionalIds> Relic2BonusIds { get; set; }

    public ICollection<ItemStringAdditionalIds> Relic3BonusIds { get; set; }

    public string? CrafterGuid { get; set; }

    public string? ExtraEnchantId { get; set; }

    public string InputString { get; set; }
}

public class ItemStringAdditionalIds
{
    [Key]
    public int ItemStringAdditionalId { get; set; }

    public string AdditionalId { get; set; }

    public AdditionalIdType Type { get; set; }
}

public class Modifier
{
    [Key]
    public int ModifierId { get; set; }

    public string ModifierType { get; set; }

    public string ModifierValue { get; set; }
}