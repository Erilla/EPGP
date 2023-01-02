using EPGP.Data.DbContexts;

namespace EPGP.Data.Repositories
{
    public class UploadHistoryRepository : IUploadHistoryRepository
    {
        private readonly EPGPContext _epgpContext;

        public UploadHistoryRepository(EPGPContext epgpContext) => (_epgpContext) = (epgpContext);


        public IEnumerable<UploadHistory> GetAllUploadHistory() => _epgpContext.UploadHistories;

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
