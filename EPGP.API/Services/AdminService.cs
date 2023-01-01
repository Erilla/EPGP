using EPGP.Data.DbContexts;
using EPGP.Data.Enums;
using EPGP.Data.Repositories;

namespace EPGP.API.Services
{
    public class AdminService : IAdminService
    {
        private readonly EPGPContext _epgpContext;
        private readonly IRaiderRepository _raiderRepository;
        private readonly IRaiderIoService _raiderIoService;

        public AdminService(EPGPContext epgpContext, IRaiderRepository raiderRepository, IRaiderIoService raiderIoService)
        {
            _epgpContext = epgpContext;
            _raiderRepository = raiderRepository;
            _raiderIoService = raiderIoService;
        }

        public void CreateDatabases()
        {
            _epgpContext.Database.EnsureCreated();
        }

        public void DeleteDatabases()
        {
            _epgpContext.Database.EnsureDeleted();
        }

        public async Task FillRaiderDetails()
        {
            var raiders = _raiderRepository.GetAllRaiders();
            var unknownClassRaiders = raiders.Where(r => r.Class == Data.Enums.Class.Unknown);
            foreach (var raider in unknownClassRaiders)
            {
                var characterProfile = await _raiderIoService.GetCharactersProfile(raider.Region, raider.Realm, raider.CharacterName);

                raider.Class = (Class)Enum.Parse(typeof(Class), characterProfile.Class, true);

                _raiderRepository.UpdateRaider(raider);
            }
        }
    }
}
