using EPGP.Data.DbContexts;

namespace EPGP.Data.Repositories
{
    public interface IPointsRepository
    {
        (int, int) CreatePoints(int raiderId);

        void UpdateEffortPoints(int raiderId, decimal points);

        void UpdateGearPoints(int raiderId, decimal points);

        EffortPoints? GetLatestEffortPoints(int raiderId);

        IEnumerable<EffortPoints> GetAllEffortPoints();

        GearPoints? GetLatestGearPoints(int raiderId);

        IEnumerable<GearPoints> GetAllGearPoints();
    }
}
