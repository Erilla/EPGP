using EPGP.API.Models;

namespace EPGP.API.Services
{
    public interface ILootService
    {
        LootHistory GetLootHistory(int raiderId, int pageSize);
    }
}
