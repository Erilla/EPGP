using EPGP.API.Models;
using EPGP.API.Responses;

namespace EPGP.API.Services;
public interface IPointsService
{
    Raider GetPoints(int raiderId);

    AllRaiderPointsResponse? GetAllPoints(DateTime? cutoffDate);

    void UpdateEffortPoints(int raiderId, decimal points);

    void UpdateGearPoints(int raiderId, decimal points);
}
