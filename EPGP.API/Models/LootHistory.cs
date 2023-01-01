namespace EPGP.API.Models;

public class LootHistory
{
    public int RaiderId { get; set; }

    public int TotalNumberOfLoots { get; set; }

    public IEnumerable<Loot> Loots { get; set; }
}

public class Loot
{
    public int LootHistoryId { get; set; }

    public DateTime Timestamp { get; set; }

    public ItemString ItemString { get; set; }

    public decimal GearPoints { get; set; }
}