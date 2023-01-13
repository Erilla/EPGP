using EPGP.Data.DbContexts;
using EPGP.Data.Enums;

namespace EPGP.Data.Repositories
{
    public interface ILootHistoryRepository
    {
        IEnumerable<LootHistoryMatch> GetLootHistoryForRaider(int raiderId);

        (IEnumerable<LootHistoryMatch>, int) GetPagedLootHistoryForRaider(int raiderId, int pageSize);

        (IEnumerable<LootHistoryMatch>, int, int) GetPagedLootHistoryForRaider(Region region, string realm, string characterName, int pageSize);

        (IEnumerable<DateOnly>, DateOnly, IEnumerable<LootHistoryMatch>) GetPagedLootHistoryByDate(int page);

        IEnumerable<LootHistoryMatch> GetLootHistoryMatchByDate(DateOnly date);

        int AddLootHistoryMatch(LootHistoryMatch lootHistoryMatch);

        void UpdateLootHistoryMatch(LootHistoryMatch lootHistoryMatch);

        int AddLootHistoryGearPoints(LootHistoryGearPoints lootHistoryGearPoints);

        void UpdateLootHistoryGearPoints(LootHistoryGearPoints lootHistoryGearPoints);

        int AddLootHistoryDetailed(LootHistoryDetailed lootHistoryDetailed);

        void UpdateLootHistoryDetailed(LootHistoryDetailed lootHistoryDetailed);

        void AddItemStringAdditionalIds(ICollection<ItemStringAdditionalIds> itemStringAdditionalIds);

        void AddModifiers(ICollection<Modifier> modifiers);

        int AddItemString(Data.DbContexts.ItemString itemString);
    }
}
