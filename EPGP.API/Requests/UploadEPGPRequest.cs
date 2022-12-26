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
}
