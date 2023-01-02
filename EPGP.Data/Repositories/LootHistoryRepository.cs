using EPGP.Data.DbContexts;
using EPGP.Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace EPGP.Data.Repositories
{
    public class LootHistoryRepository : ILootHistoryRepository
    {
        private readonly EPGPContext _epgpContext;
        private readonly IRaiderRepository _raiderRepository;
        public LootHistoryRepository(EPGPContext epgpContext, IRaiderRepository raiderRepository) => (_epgpContext, _raiderRepository) = (epgpContext, raiderRepository);

        public IEnumerable<LootHistoryMatch> GetLootHistoryForRaider(int raiderId)
        {
            return _epgpContext.LootHistoryMatch
                .Where(lh => lh.RaiderId == raiderId)
                .Include(lh => lh.LootHistoryGearPoints)
                .Include(lh => lh.LootHistoryDetailed)
                .ToList();
        }

        public (IEnumerable<LootHistoryMatch>, int) GetPagedLootHistoryForRaider(int raiderId, int pageSize)
        {
            var result = _epgpContext.LootHistoryMatch
                .Where(lh => lh.RaiderId == raiderId)
                .OrderByDescending(lh => lh.Date)

                .Include(lh => lh.LootHistoryGearPoints)
                    .ThenInclude(x => x.ItemString)
                    .ThenInclude(x => x.BonusIds)
                .Include(lh => lh.LootHistoryGearPoints.ItemString)
                    .ThenInclude(x => x.Modifiers)
                .Include(lh => lh.LootHistoryGearPoints.ItemString)
                    .ThenInclude(x => x.Relic1BonusIds)
                .Include(lh => lh.LootHistoryGearPoints.ItemString)
                    .ThenInclude(x => x.Relic2BonusIds)
                .Include(lh => lh.LootHistoryGearPoints.ItemString)
                    .ThenInclude(x => x.Relic3BonusIds)

                .Include(lh => lh.LootHistoryDetailed)
                    .ThenInclude(x => x.ItemString)
                    .ThenInclude(x => x.BonusIds)
                .Include(lh => lh.LootHistoryDetailed.ItemString)
                    .ThenInclude(x => x.Modifiers)
                .Include(lh => lh.LootHistoryDetailed.ItemString)
                    .ThenInclude(x => x.Relic1BonusIds)
                .Include(lh => lh.LootHistoryDetailed.ItemString)
                    .ThenInclude(x => x.Relic2BonusIds)
                .Include(lh => lh.LootHistoryDetailed.ItemString)
                    .ThenInclude(x => x.Relic3BonusIds)

                .ToList();

            return (result.Take(pageSize), result.Count);
        }

        public int AddLootHistoryMatch(LootHistoryMatch lootHistoryMatch)
        {
            _epgpContext.LootHistoryMatch.Add(lootHistoryMatch);
            _epgpContext.SaveChanges();
            return lootHistoryMatch.LootHistoryMatchId;
        }

        public void UpdateLootHistoryMatch(LootHistoryMatch lootHistoryMatch)
        {
            _epgpContext.LootHistoryMatch.Update(lootHistoryMatch);
            _epgpContext.SaveChanges();
        }

        public int AddLootHistoryGearPoints(LootHistoryGearPoints lootHistoryGearPoints)
        {
            _epgpContext.LootHistoryGearPoints.Add(lootHistoryGearPoints);
            _epgpContext.SaveChanges();
            return lootHistoryGearPoints.LootHistoryGearPointsId;
        }

        public void UpdateLootHistoryGearPoints(LootHistoryGearPoints lootHistoryGearPoints)
        {
            _epgpContext.LootHistoryGearPoints.Update(lootHistoryGearPoints);
            _epgpContext.SaveChanges();
        }

        public int AddLootHistoryDetailed(LootHistoryDetailed lootHistoryDetailed)
        {
            _epgpContext.LootHistoryDetailed.Add(lootHistoryDetailed);
            _epgpContext.SaveChanges();
            return lootHistoryDetailed.LootHistoryDetailedId;
        }

        public void UpdateLootHistoryDetailed(LootHistoryDetailed lootHistoryDetailed)
        {
            _epgpContext.LootHistoryDetailed.Update(lootHistoryDetailed);
            _epgpContext.SaveChanges();
        }

        public IEnumerable<LootHistoryMatch> GetLootHistoryMatchByDate(DateOnly date)
        {
            return _epgpContext.LootHistoryMatch
                .Where(lhm => DateOnly.FromDateTime(lhm.Date) == date)
                .Include(lhm => lhm.Raider)
                .Include(lhm => lhm.LootHistoryGearPoints)
                    .ThenInclude(x => x.ItemString)
                .Include(lhm => lhm.LootHistoryDetailed)
                .ToList();
        }

        public void AddItemStringAdditionalIds(ICollection<ItemStringAdditionalIds> itemStringAdditionalIds)
        {
            if (!itemStringAdditionalIds.Except(_epgpContext.ItemStringAdditionalIds).Any())
            {
                var newIds = itemStringAdditionalIds.Where(i => _epgpContext.ItemStringAdditionalIds.Any(j => i.AdditionalId == j.AdditionalId));
                _epgpContext.ItemStringAdditionalIds.AddRange(newIds);
                _epgpContext.SaveChanges();
            }
        }

        public int AddItemString(ItemString itemString)
        {
            var id = -1;

            if (_epgpContext.ItemStrings.Any(i => i.InputString == itemString.InputString))
            {
                id = _epgpContext.ItemStrings.First(i => i.InputString == itemString.InputString).ItemStringId;
            }
            else
            {
                _epgpContext.ItemStrings.Add(itemString);
                _epgpContext.SaveChanges();

                id = itemString.ItemStringId;
            }

            return id;
        }

        public void AddModifiers(ICollection<Modifier> modifiers)
        {
            if (!modifiers.Except(_epgpContext.Modifiers).Any())
            {
                var newModifiers = modifiers.Where(i => _epgpContext.Modifiers.Any(j => j.ModifierType == i.ModifierType && j.ModifierValue == i.ModifierValue));
                _epgpContext.Modifiers.AddRange(newModifiers);
                _epgpContext.SaveChanges();
            }
        }

        public (IEnumerable<LootHistoryMatch>, int, int) GetPagedLootHistoryForRaider(Region region, string realm, string characterName, int pageSize)
        {
            var raider = _raiderRepository.GetRaider(characterName, realm, region).FirstOrDefault();

            if (raider == null) throw new ArgumentException("Unable to find raider");

            var (result, totalCount) = GetPagedLootHistoryForRaider(raider.RaiderId, pageSize);

            return (result, totalCount, raider.RaiderId);
        }
    }
}
