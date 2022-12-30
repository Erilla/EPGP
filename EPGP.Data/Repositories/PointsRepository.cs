using EPGP.Data.DbContexts;

namespace EPGP.Data.Repositories
{
    public class PointsRepository : IPointsRepository
    {
        private readonly EPGPContext _epgpContext;

        public PointsRepository(EPGPContext epgpContext) => (_epgpContext) = epgpContext;

        public (int, int) CreatePoints(int raiderId)
        {
            var effortPoints = new EffortPoints
            {
                RaiderId = raiderId,
                Points = 0
            };
            _epgpContext.EffortPoints.Add(effortPoints);
            _epgpContext.SaveChanges();

            var gearPoints = new GearPoints
            {
                RaiderId = raiderId,
                Points = 0
            };
            _epgpContext.GearPoints.Add(gearPoints);
            _epgpContext.SaveChanges();

            return (effortPoints.EffortPointsId, gearPoints.GearPointsId);
        }

        public EffortPoints? GetEffortPoints(int raiderId)
        {
            return _epgpContext.EffortPoints.SingleOrDefault(s => s.RaiderId == raiderId);
        }

        public IEnumerable<EffortPoints> GetAllEffortPoints()
        {
            return _epgpContext.EffortPoints;
        }

        public GearPoints? GetGearPoints(int raiderId)
        {
            return _epgpContext.GearPoints.SingleOrDefault(s => s.RaiderId == raiderId);
        }

        public IEnumerable<GearPoints> GetAllGearPoints()
        {
            return _epgpContext.GearPoints;
        }

        public void UpdateEffortPoints(int raiderId, decimal points)
        {
            var effortPoints = GetEffortPoints(raiderId);
            if (effortPoints == null) throw new ArgumentException($"Unable to find effort points for Raider Id {raiderId}");

            effortPoints.Points = points;
            _epgpContext.EffortPoints.Update(effortPoints);
            _epgpContext.SaveChanges();
        }

        public void UpdateGearPoints(int raiderId, decimal points)
        {
            var gearPoints = GetGearPoints(raiderId);
            if (gearPoints == null) throw new ArgumentException($"Unable to find effort points for Raider Id {raiderId}");

            gearPoints.Points = points;
            _epgpContext.GearPoints.Update(gearPoints);
            _epgpContext.SaveChanges();
        }
    }
}
