using EPGP.Data.DbContexts;

namespace EPGP.Data.Repositories
{
    public interface IUploadHistoryRepository
    {
        DateTime? GetLatestUploadDateTime();

        void SaveUploadHistory(UploadHistory uploadHistory);

        IEnumerable<UploadHistory> GetAllUploadHistory();
    }
}
