using EPGP.API.Models;

namespace EPGP.API.Services
{
    public interface IRaiderService
    {
        Raider GetRaider(int raiderId);

        Raider GetRaider(string characterName, string realm);

        int CreateRaider(Raider rader);

        void CreateRaiders(IEnumerable<Raider> raiders);

        void UpdateRaider(Raider rader);

        void DeleteRaider(int raiderId);
    }
}
