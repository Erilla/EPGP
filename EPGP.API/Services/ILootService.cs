using EPGP.API.Models;
using EPGP.Data.DbContexts;

namespace EPGP.API.Services
{
    public interface ILootService
    {
        LootHistory GetLootHistory(int raiderId, int pageSize);

        void AddItemStringAdditionalIds(ICollection<ItemStringAdditionalIds> itemStringAdditionalIds);

        void AddModifiers(ICollection<Data.DbContexts.Modifier> modifiers);

        int AddItemString(Data.DbContexts.ItemString itemString);
    }
}
