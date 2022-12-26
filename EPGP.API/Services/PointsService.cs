using EPGP.API.Models;
using EPGP.Data.Repositories;

namespace EPGP.API.Services
{
    public class PointsService : IPointsService
    {
        private readonly IPointsRepository _pointsRepository;
        private readonly IRaiderRepository _raiderRepository;

        public PointsService(IPointsRepository pointsRepository, IRaiderRepository raiderRepository) => (_pointsRepository, _raiderRepository) = (pointsRepository, raiderRepository);

        public Points GetPoints(int raiderId)
        {
            var effortPoints = _pointsRepository.GetEffortPoints(raiderId);
            var gearPoints = _pointsRepository.GetGearPoints(raiderId);

            return new Points
            {
                RaiderId = raiderId,
                EffortPoints = effortPoints == null ? 0 : effortPoints.Points,
                GearPoints = gearPoints == null ? 0 : gearPoints.Points
            };
        }

        public IEnumerable<Points> GetAllPoints()
        {
            var raiders = _raiderRepository.GetAllRaiders();

            return raiders.Select(r =>
            {
                return new Points
                {
                    RaiderId = r.RaiderId,
                    EffortPoints = r.EffortPoints?.Points ?? 0,
                    GearPoints = r.GearPoints?.Points ?? 0
                };
            });
        }

        public void UpdateEffortPoints(int raiderId, int points) => _pointsRepository.UpdateEffortPoints(raiderId, points);

        public void UpdateGearPoints(int raiderId, int points) => _pointsRepository.UpdateGearPoints(raiderId, points);
    }
}
