using AutoMapper;
using EPGP.Data.DbContexts;
using EPGP.Data.Repositories;

namespace EPGP.API.Services
{
    public class RaiderService : IRaiderService
    {
        private readonly IRaiderRepository _raiderRepository;
        private readonly IMapper _mapper;

        public RaiderService(IRaiderRepository raiderRepository, IMapper mapper) => (_raiderRepository, _mapper) = (raiderRepository, mapper);

        public int? CreateRaider(Models.Raider raider)
        {
            var existingRaider = _raiderRepository.GetRaider(raider.CharacterName, raider.Realm, raider.Region, raider.Class);

            if (existingRaider.Any())
            {
                return null;
            }

            return _raiderRepository.CreateRaider(_mapper.Map<Raider>(raider));
        }


        public void CreateRaiders(IEnumerable<Models.Raider> raiders)
        {
            _ = _raiderRepository.CreateRaiders(
                raiders.Select(r => _mapper.Map<Raider>(r))
            );
        }

        public void DeleteRaider(int raiderId)
        {
            _raiderRepository.DeleteRaider(raiderId);
        }

        public Models.Raider? GetRaider(int raiderId)
        {
            var raider = _raiderRepository.GetRaider(raiderId);
            if (raider == null) return null;
            return _mapper.Map<Models.Raider>(raider);
        }

        public Models.Raider? GetRaider(string characterName, string realm)
        {
            var raider = _raiderRepository.GetRaider(characterName, realm).FirstOrDefault();
            if (raider == null) return null;
            return _mapper.Map<Models.Raider>(raider);
        }

        public void SetAllRaidersInactive()
        {
            var raiders = _raiderRepository.GetAllRaiders();
            foreach (var raider in raiders)
            {
                raider.Active = false;
                _raiderRepository.UpdateRaider(raider);
            }
        }

        public void SetRaiderActivity(int raiderId, bool active)
        {
            var raider = _raiderRepository.GetRaider(raiderId);
            raider.Active = active;
            _raiderRepository.UpdateRaider(raider);
        }

        public void UpdateRaider(Models.Raider raider)
        {
            // Does not work
            _raiderRepository.UpdateRaider(_mapper.Map<Raider>(raider));
        }
    }
}
