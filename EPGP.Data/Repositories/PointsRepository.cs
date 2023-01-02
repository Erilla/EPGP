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
                Timestamp = DateTime.Now.ToUniversalTime(),
                RaiderId = raiderId,
                Points = 0
            };
            _epgpContext.EffortPoints.Add(effortPoints);
            _epgpContext.SaveChanges();

            var gearPoints = new GearPoints
            {
                Timestamp = DateTime.Now.ToUniversalTime(),
                RaiderId = raiderId,
                Points = 0
            };
            _epgpContext.GearPoints.Add(gearPoints);
            _epgpContext.SaveChanges();

            return (effortPoints.EffortPointsId, gearPoints.GearPointsId);
        }

        public EffortPoints? GetLatestEffortPoints(int raiderId)
        {
            return _epgpContext.EffortPoints.OrderByDescending(ep => ep.Timestamp).FirstOrDefault(s => s.RaiderId == raiderId);
        }

        public IEnumerable<EffortPoints> GetAllEffortPoints()
        {
            return _epgpContext.EffortPoints;
        }

        public GearPoints? GetLatestGearPoints(int raiderId)
        {
            return _epgpContext.GearPoints.OrderByDescending(ep => ep.Timestamp).FirstOrDefault(s => s.RaiderId == raiderId);
        }

        public IEnumerable<GearPoints> GetAllGearPoints()
        {
            return _epgpContext.GearPoints;
        }

        public void UpdateEffortPoints(int raiderId, decimal points)
        {
            var newEffortPoints = new EffortPoints
            {
                Timestamp = DateTime.Now.ToUniversalTime(),
                Points = points,
                RaiderId = raiderId
            };

            _epgpContext.EffortPoints.Add(newEffortPoints);
            _epgpContext.SaveChanges();
        }

        public void UpdateGearPoints(int raiderId, decimal points)
        {
            var latestGearPoints = GetLatestGearPoints(raiderId);

            var newGearPoints = new GearPoints
            {
                Timestamp = DateTime.Now.ToUniversalTime(),
                Points = points,
                RaiderId = raiderId
            };

            _epgpContext.GearPoints.Add(newGearPoints);
            _epgpContext.SaveChanges();
        }
    }
}
