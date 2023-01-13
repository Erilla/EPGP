using EPGP.API.Models;
using EPGP.Data.Enums;

namespace EPGP.API.Responses
{
    public class LootHistoryByDateResponse
    {
        public IEnumerable<DateOnly> RaidDates { get; set; }

        public LootHistoryByDate LootHistory { get; set; }
    }

    public class LootHistoryByDate
    {
        public DateOnly RaidDate { get; set; }

        public IEnumerable<LootByDate> Loot { get; set; }
    }

    public class LootByDate
    {
        public ItemString ItemString { get; set; }

        public decimal GearPoints { get; set; }

        public string CharacterName { get; set; }

        public Region Region { get; set; }

        public string Realm { get; set; }

        public Class CharacterClass { get; set; }
    }
}
