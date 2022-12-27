using EPGP.Data.Repositories;

namespace EPGP.API.Services
{
    public class RaiderService : IRaiderService
    {
        private readonly IRaiderRepository _raiderRepository;

        public RaiderService(IRaiderRepository raiderRepository) => (_raiderRepository) = (raiderRepository);

        public int CreateRaider(Models.Raider raider)
        {
            var existingRaider = _raiderRepository.GetRaider(raider.CharacterName, raider.Realm, raider.Region, raider.Class);

            if (existingRaider.Any())
            {
                return existingRaider.First().RaiderId;
            }

            return _raiderRepository.CreateRaider(new Data.DbContexts.Raider
            {
                CharacterName = raider.CharacterName,
                Region = raider.Region,
                Realm = raider.Realm,
                Class = raider.Class
            });
        }

        public void CreateRaiders(IEnumerable<Models.Raider> raiders)
        {
            _raiderRepository.CreateRaiders(
                raiders.Select(r => new Data.DbContexts.Raider
                {
                    CharacterName = r.CharacterName,
                    Region = r.Region,
                    Realm = r.Realm,
                    Class = r.Class
                })
            );
        }

        public void DeleteRaider(int raiderId)
        {
            _raiderRepository.DeleteRaider(raiderId);
        }

        public Models.Raider GetRaider(int raiderId)
        {
            var raider = _raiderRepository.GetRaider(raiderId);
            return new Models.Raider
            {
                RaiderId = raider.RaiderId,
                CharacterName = raider.CharacterName,
                Class = raider.Class,
                Realm = raider.Realm,
                Region = raider.Region,
            };
        }

        public void UpdateRaider(Models.Raider raider)
        {
            throw new NotImplementedException();
        }
    }
}
