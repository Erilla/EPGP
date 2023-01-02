using EPGP.Data.DbContexts;
using EPGP.Data.Enums;

namespace EPGP.Data.Repositories
{
    public interface IRaiderRepository
    {
        int CreateRaider(Raider raider);

        IEnumerable<Raider> GetRaider(string characterName, string realm = "", Region region = Region.Unknown, Class characterClass = Class.Unknown);

        IEnumerable<int> CreateRaiders(IEnumerable<Raider> raiders);

        void UpdateRaider(Raider raider);

        Raider? GetRaider(int raiderId);

        void DeleteRaider(int raiderId);

        IEnumerable<Raider> GetAllRaiders();
    }
}
