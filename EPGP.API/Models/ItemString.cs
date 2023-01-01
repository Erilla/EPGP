namespace EPGP.API.Models
{
    // More info here: https://wowpedia.fandom.com/wiki/ItemLink
    public class ItemString
    {
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

        public IEnumerable<string> BonusIds { get; set; }

        public IEnumerable<Modifier> Modifiers { get; set; }

        public IEnumerable<string> Relic1BonusIds { get; set; }

        public IEnumerable<string> Relic2BonusIds { get; set; }

        public IEnumerable<string> Relic3BonusIds { get; set; }

        public string? CrafterGuid { get; set; }

        public string? ExtraEnchantId { get; set; }
    }

    public class Modifier
    {
        public string ModifierType { get; set; }

        public string ModifierValue { get; set; }
    }
}
