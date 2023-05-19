using EPGP.Data.DbContexts;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EPGP.Data.Repositories
{
    public class UploadHistoryRepository : IUploadHistoryRepository
    {
        private readonly EPGPContext _epgpContext;

        public UploadHistoryRepository(EPGPContext epgpContext) => (_epgpContext) = (epgpContext);


        public IEnumerable<UploadHistory> GetAllUploadHistory() => _epgpContext.UploadHistories;

        public decimal GetLatestDecay()
        {
            var latest = _epgpContext.UploadHistories.OrderByDescending(up => up.Timestamp).FirstOrDefault();
            if (latest == null) return 0;

            JObject? latestJsonObject = JsonConvert.DeserializeObject(latest.UploadedContent) as JObject;

            if (latestJsonObject.HasValues)
            {
                var decayPercent = latestJsonObject["decay_p"]?.Value<decimal>();

                return decayPercent ?? 0;
            }

            return 0;
        }

        public DateTime? GetLatestUploadDateTime()
        {
            var latest = _epgpContext.UploadHistories.OrderByDescending(up => up.Timestamp).FirstOrDefault();
            if (latest == null) return null;
            return latest.Timestamp;
        }

        public void SaveUploadHistory(UploadHistory uploadHistory)
        {
            _epgpContext.UploadHistories.Add(uploadHistory);
            _epgpContext.SaveChanges();
        }
    }
}
