namespace EPGP.API.Services
{
    public interface IAdminService
    {
        void CreateDatabases();

        void DeleteDatabases();

        Task FillRaiderDetails();
    }
}
