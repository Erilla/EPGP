using EPGP.Data.Enums;

namespace EPGP.API.Models
{
    public class Raider
    {
        public int RaiderId { get; set; }

        public string CharacterName { get; set; }

        public Region Region { get; set; }

        public string Realm { get; set; }

        public Class Class { get; set; }
    }
}
