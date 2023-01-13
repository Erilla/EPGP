using EPGP.API.Models;
using EPGP.API.Responses;
using EPGP.Data.DbContexts;
using EPGP.Data.Enums;

namespace EPGP.API.Services
{
    public interface ILootService
    {
        LootHistory GetLootHistory(int raiderId, int pageSize);

        LootHistory GetLootHistory(Region region, string realm, string characterName, int pageSize);

        LootHistoryByDateResponse GetLootHistoryByDate(int page);

        void AddItemStringAdditionalIds(ICollection<ItemStringAdditionalIds> itemStringAdditionalIds);

        void AddModifiers(ICollection<Data.DbContexts.Modifier> modifiers);

        int AddItemString(Data.DbContexts.ItemString itemString);
    }
}
