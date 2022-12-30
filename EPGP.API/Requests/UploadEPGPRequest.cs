namespace EPGP.API.Requests
{
    public class UploadEPGPRequest
    {
        public string Guild { get; set; }
        public string Region { get; set; }
        public int Min_ep { get; set; }
        public int Base_gp { get; set; }
        public object[][] Roster { get; set; }
        public int Decay_p { get; set; }
        public object[][] Loot { get; set; }
        public int Timestamp { get; set; }
        public int Extras_p { get; set; }
        public string Realm { get; set; }
    }

    public class UploadEPGPRoster
    {
        public string CharacterName { get; set; }

        public string Realm { get; set; }

        public decimal EffortPoints { get; set; }

        public decimal GearPoints { get; set; }
    }

    public class UploadEPGPLoot
    {
        public DateTime Timestamp { get; set; }

        public string CharacterName { get; set; }

        public string Realm { get; set; }

        public string ItemString { get; set; }

        public decimal GearPoints { get; set; }
    }
}
