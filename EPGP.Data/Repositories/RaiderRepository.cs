using EPGP.Data.DbContexts;
using EPGP.Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace EPGP.Data.Repositories;

public class RaiderRepository : IRaiderRepository
{
    private readonly IPointsRepository _pointsRepository;
    private readonly EPGPContext _epgpContext;

    public RaiderRepository(IPointsRepository pointsRepository, EPGPContext epgpContext) => (_pointsRepository, _epgpContext) = (pointsRepository, epgpContext);

    public int CreateRaider(Raider raider)
    {
        _epgpContext.Raiders.Add(raider);
        _epgpContext.SaveChanges();

        _pointsRepository.CreatePoints(raider.RaiderId);

        return raider.RaiderId;
    }

    public void UpdateRaider(Raider raider)
    {
        _epgpContext.Raiders.Update(raider);
        _epgpContext.SaveChanges();
    }

    public IEnumerable<Raider> GetAllRaiders()
    {
        return _epgpContext.Raiders
            .Include(r => r.EffortPoints)
            .Include(r => r.GearPoints)
            .ToList();
    }

    public Raider? GetRaider(int raiderId)
    {
        return _epgpContext.Raiders
            .SingleOrDefault(r => r.RaiderId == raiderId);
    }

    public IEnumerable<Raider> GetRaider(string characterName, string realm, Region region = Region.Unknown, Class characterClass = Class.Unknown)
    {
        return _epgpContext.Raiders
            .Where(r =>
                 r.CharacterName.ToLower() == characterName.ToLower() &&
                 r.Realm.ToLower() == realm.ToLower() &&
                 (region == Region.Unknown || r.Region == region) &&
                 (characterClass == Class.Unknown || r.Class == characterClass))
            .ToList();
    }

    public IEnumerable<int> CreateRaiders(IEnumerable<Raider> raiders)
    {
        var raiderIds = new List<int>();

        foreach (var raider in raiders)
        {
            raiderIds.Add(CreateRaider(raider));
        }

        return raiderIds;
    }

    public void DeleteRaider(int raiderId)
    {
        var raider = GetRaider(raiderId);
        if (raider == null) return;

        _epgpContext.Remove(raider);
        _epgpContext.SaveChanges();
    }
}
