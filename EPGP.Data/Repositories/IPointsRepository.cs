using EPGP.Data.DbContexts;

namespace EPGP.Data.Repositories
{
    public interface IPointsRepository
    {
        (int, int) CreatePoints(int raiderId);

        void UpdateEffortPoints(int raiderId, decimal points);

        void UpdateGearPoints(int raiderId, decimal points);

        EffortPoints? GetEffortPoints(int raiderId);

        IEnumerable<EffortPoints> GetAllEffortPoints();

        GearPoints? GetGearPoints(int raiderId);

        IEnumerable<GearPoints> GetAllGearPoints();
    }
}
