namespace EPGP.API.Models;

public class LootHistory
{
    public int RaiderId { get; set; }

    public int TotalNumberOfLoots { get; set; }

    public IEnumerable<Loot> Loots { get; set; }
}

public class Loot
{
    public DateTime Timestamp { get; set; }

    public string ItemString { get; set; }

    public decimal GearPoints { get; set; }
}