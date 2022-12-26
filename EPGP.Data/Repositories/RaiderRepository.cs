using EPGP.Data.DbContexts;
using EPGP.Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace EPGP.Data.Repositories;

public class RaiderRepository : IRaiderRepository
{
    private readonly IPointsRepository _pointsRepository;

    public RaiderRepository(IPointsRepository pointsRepository) => (_pointsRepository) = (pointsRepository);

    public int CreateRaider(Raider raider)
    {
        using var context = new RaiderContext();
        context.Raiders.Add(raider);
        context.SaveChanges();

        _pointsRepository.CreatePoints(raider.RaiderId);

        return raider.RaiderId;
    }

    public void UpdateRaider(Raider raider)
    {
        using var context = new RaiderContext();
        context.Raiders.Update(raider);
        context.SaveChanges();
    }

    public IEnumerable<Raider> GetAllRaiders()
    {
        using var context = new RaiderContext();
        return context.Raiders
            .Include(r => r.EffortPoints)
            .Include(r => r.GearPoints)
            .ToList();
    }

    public Raider GetRaider(int raiderId)
    {
        using var context = new RaiderContext();
        return context.Raiders
            .Single(r => r.RaiderId == raiderId);
    }

    public IEnumerable<Raider> GetRaider(string characterName, string realm = "", Region region = Region.Unknown, Class characterClass = Class.Unknown)
    {
        using var context = new RaiderContext();
        return context.Raiders
            .Where(r =>
                 r.CharacterName == characterName &&
                 (realm == "" || r.Realm == realm) &&
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
        throw new NotImplementedException();
    }
}
