using EPGP.API.Models;
using EPGP.API.Responses;
using EPGP.Data.Enums;

namespace EPGP.API.Services;
public interface IPointsService
{
    Raider GetPoints(int raiderId);

    AllRaiderPointsResponse? GetAllPoints(DateTime? cutoffDate, DateTime? toDate = null, TierToken? tierToken = null, ArmourType? armourType = null);

    void UpdateEffortPoints(int raiderId, decimal points);

    void UpdateGearPoints(int raiderId, decimal points);
}
