namespace EPGP.API.Responses
{
    public class UploadEPGPResponse
    {
        public string RosterJobId { get; set; }

        public IEnumerable<string> LootJobIds { get; set; }
    }
}
