using EPGP.API.Models;
using EPGP.Data.Repositories;

namespace EPGP.API.Services
{
    public class PointsService : IPointsService
    {
        private readonly IPointsRepository _pointsRepository;
        private readonly IRaiderRepository _raiderRepository;

        public PointsService(IPointsRepository pointsRepository, IRaiderRepository raiderRepository) => (_pointsRepository, _raiderRepository) = (pointsRepository, raiderRepository);

        public Raider GetPoints(int raiderId)
        {
            var raider = _raiderRepository.GetRaider(raiderId);
            var effortPoints = _pointsRepository.GetEffortPoints(raiderId);
            var gearPoints = _pointsRepository.GetGearPoints(raiderId);

            return new Raider
            {
                RaiderId = raiderId,
                CharacterName = raider.CharacterName,
                Region = raider.Region,
                Realm = raider.Realm,
                Class = raider.Class,
                Points = new Points
                {
                    EffortPoints = effortPoints == null ? 0 : effortPoints.Points,
                    GearPoints = gearPoints == null ? 0 : gearPoints.Points
                }
            };
        }

        public IEnumerable<Raider> GetAllPoints()
        {
            var raiders = _raiderRepository.GetAllRaiders();

            return raiders
                .Select(r =>
                    {
                        return new Raider
                        {
                            RaiderId = r.RaiderId,
                            CharacterName = r.CharacterName,
                            Region = r.Region,
                            Realm = r.Realm,
                            Class = r.Class,
                            Points = new Points
                            {
                                EffortPoints = r.EffortPoints?.Points ?? 0,
                                GearPoints = r.GearPoints?.Points ?? 0
                            }
                        };
                    })
                .OrderByDescending(r => r.Points.Priority);
        }

        public void UpdateEffortPoints(int raiderId, int points) => _pointsRepository.UpdateEffortPoints(raiderId, points);

        public void UpdateGearPoints(int raiderId, int points) => _pointsRepository.UpdateGearPoints(raiderId, points);
    }
}
