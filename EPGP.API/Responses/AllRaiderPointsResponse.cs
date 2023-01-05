using EPGP.API.Models;

namespace EPGP.API.Responses
{
    public class AllRaiderPointsResponse
    {
        public DateTime LastUploadedDate { get; internal set; }
        public DateTime CutOffDate { get; internal set; }
        public IEnumerable<Raider> Raiders { get; set; }
    }
}
