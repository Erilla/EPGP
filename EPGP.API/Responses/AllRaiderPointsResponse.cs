using EPGP.API.Models;

namespace EPGP.API.Responses
{
    public class AllRaiderPointsResponse
    {
        public DateTime LastUploadedDate { get; set; }
        public DateTime CutOffDate { get; set; }
        public IEnumerable<Raider> Raiders { get; set; }
    }
}
