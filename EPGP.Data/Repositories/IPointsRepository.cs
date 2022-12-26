using EPGP.Data.DbContexts;

namespace EPGP.Data.Repositories
{
    public interface IPointsRepository
    {
        (int, int) CreatePoints(int raiderId);

        void UpdateEffortPoints(int raiderId, int points);

        void UpdateGearPoints(int raiderId, int points);

        EffortPoints? GetEffortPoints(int raiderId);

        IEnumerable<EffortPoints> GetAllEffortPoints();

        GearPoints? GetGearPoints(int raiderId);

        IEnumerable<GearPoints> GetAllGearPoints();
    }
}
