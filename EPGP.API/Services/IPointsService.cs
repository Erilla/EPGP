using EPGP.API.Models;

namespace EPGP.API.Services;
public interface IPointsService
{
    Raider GetPoints(int raiderId);

    IEnumerable<Raider> GetAllPoints();

    void UpdateEffortPoints(int raiderId, decimal points);

    void UpdateGearPoints(int raiderId, decimal points);
}
