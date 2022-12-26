using EPGP.Data.DbContexts;

namespace EPGP.Data.Repositories
{
    public class PointsRepository : IPointsRepository
    {
        public (int, int) CreatePoints(int raiderId)
        {
            using var effortPointsContext = new EffortPointsContext();
            var effortPoints = new EffortPoints
            {
                RaiderId = raiderId,
                Points = 0
            };
            effortPointsContext.EffortPoints.Add(effortPoints);
            effortPointsContext.SaveChanges();

            using var gearPointsContext = new GearPointsContext();
            var gearPoints = new GearPoints
            {
                RaiderId = raiderId,
                Points = 0
            };
            gearPointsContext.GearPoints.Add(gearPoints);
            gearPointsContext.SaveChanges();

            return (effortPoints.EffortPointsId, gearPoints.GearPointsId);
        }

        public EffortPoints? GetEffortPoints(int raiderId)
        {
            using var context = new EffortPointsContext();
            return context.EffortPoints.SingleOrDefault(s => s.RaiderId == raiderId);
        }

        public IEnumerable<EffortPoints> GetAllEffortPoints()
        {
            using var context = new EffortPointsContext();
            return context.EffortPoints;
        }

        public GearPoints? GetGearPoints(int raiderId)
        {
            using var context = new GearPointsContext();
            return context.GearPoints.SingleOrDefault(s => s.RaiderId == raiderId);
        }

        public IEnumerable<GearPoints> GetAllGearPoints()
        {
            using var context = new GearPointsContext();
            return context.GearPoints;
        }

        public void UpdateEffortPoints(int raiderId, int points)
        {
            using var context = new EffortPointsContext();

            var effortPoints = GetEffortPoints(raiderId);
            if (effortPoints == null) throw new ArgumentException($"Unable to find effort points for Raider Id {raiderId}");

            effortPoints.Points = points;
            context.EffortPoints.Update(effortPoints);
            context.SaveChanges();
        }

        public void UpdateGearPoints(int raiderId, int points)
        {
            using var context = new GearPointsContext();

            var gearPoints = GetGearPoints(raiderId);
            gearPoints.Points = points;
            context.GearPoints.Update(gearPoints);
            context.SaveChanges();
        }
    }
}
