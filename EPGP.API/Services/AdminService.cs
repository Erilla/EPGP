using EPGP.Data.DbContexts;

namespace EPGP.API.Services
{
    public class AdminService : IAdminService
    {
        private readonly EPGPContext _epgpContext;

        public AdminService(EPGPContext epgpContext)
        {
            _epgpContext = epgpContext;
        }

        public void CreateDatabases()
        {
            _epgpContext.Database.EnsureCreated();
        }
    }
}
