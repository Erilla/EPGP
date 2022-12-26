using EPGP.API.Models;

namespace EPGP.API.Services;
public interface IPointsService
{
    Points GetPoints(int raiderId);

    IEnumerable<Points> GetAllPoints();

    void UpdateEffortPoints(int raiderId, int points);

    void UpdateGearPoints(int raiderId, int points);
}
