using EPGP.API.Models;
using EPGP.API.Responses;
using EPGP.Data.Enums;
using EPGP.Data.Repositories;

namespace EPGP.API.Services
{
    public class PointsService : IPointsService
    {
        private readonly IPointsRepository _pointsRepository;
        private readonly IRaiderRepository _raiderRepository;
        private readonly IUploadHistoryRepository _uploadHistoryRepository;

        public PointsService(IPointsRepository pointsRepository, IRaiderRepository raiderRepository, IUploadHistoryRepository uploadHistoryRepository) => (_pointsRepository, _raiderRepository, _uploadHistoryRepository) = (pointsRepository, raiderRepository, uploadHistoryRepository);

        public Raider GetPoints(int raiderId)
        {
            var raider = _raiderRepository.GetRaider(raiderId);
            var effortPoints = _pointsRepository.GetLatestEffortPoints(raiderId);
            var gearPoints = _pointsRepository.GetLatestGearPoints(raiderId);

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

        public AllRaiderPointsResponse? GetAllPoints(DateTime? cutOffDate, DateTime? toDate = null, TierToken? tierToken = null, ArmourType? armourType = null)
        {
            if (!cutOffDate.HasValue && toDate.HasValue) throw new ArgumentException("No Cutoff/From date");

            if (!toDate.HasValue) toDate = DateTime.Now;
            if (!cutOffDate.HasValue) cutOffDate = CalculateCutoffDate();

            var lastUploadedDate = _uploadHistoryRepository.GetLatestUploadDateTime();
            if (lastUploadedDate == null) return null;

            var raiders = _raiderRepository.GetAllRaiders();

            if (tierToken.HasValue)
            {
                var classes = RetrieveClasses(tierToken.Value);
                raiders = raiders.Where(r => classes.Contains(r.Class));
            }

            if (armourType.HasValue)
            {
                var classes = RetrieveClasses(armourType.Value);
                raiders = raiders.Where(r => classes.Contains(r.Class));
            }

            return new AllRaiderPointsResponse
            {
                LastUploadedDate = lastUploadedDate.Value,
                CutOffDate = cutOffDate.Value,
                Raiders = raiders
                    .Where(r => r.Active)
                    .Select(r =>
                    {
                        var effortPoints = r.EffortPoints
                            .OrderByDescending(p => p.Timestamp);
                        var gearPoints = r.GearPoints
                            .OrderByDescending(p => p.Timestamp);

                        var currentEffortPoints = effortPoints.FirstOrDefault()?.Points ?? 0;
                        var currentGearPoints = gearPoints.FirstOrDefault()?.Points ?? 0;

                        var preCutoffEffortPoints = effortPoints.Where(p => p.Timestamp < cutOffDate.Value).FirstOrDefault();
                        decimal decayedEffortPoints = currentEffortPoints;

                        var preCutoffGearPoints = gearPoints.Where(p => p.Timestamp < cutOffDate.Value).FirstOrDefault();
                        decimal decayedGearPoints = currentGearPoints;

                        var decayValue = _uploadHistoryRepository.GetLatestDecay() / 100;

                        // Get Decayed values if it's Wednesday
                        if (preCutoffEffortPoints != null)
                        {
                            decayedEffortPoints = cutOffDate.Value.DayOfWeek == DayOfWeek.Wednesday && lastUploadedDate > cutOffDate ?
                                preCutoffEffortPoints.Points - decimal.Multiply(preCutoffEffortPoints.Points, decayValue) :
                                preCutoffEffortPoints.Points;
                        }

                        if (preCutoffGearPoints != null)
                        {
                            decayedGearPoints = cutOffDate.Value.DayOfWeek == DayOfWeek.Wednesday && lastUploadedDate > cutOffDate ?
                                preCutoffGearPoints.Points - decimal.Multiply(preCutoffGearPoints.Points, decayValue) :
                                preCutoffGearPoints.Points;
                        }

                        return new Raider
                        {
                            RaiderId = r.RaiderId,
                            CharacterName = r.CharacterName,
                            Region = r.Region,
                            Realm = r.Realm,
                            Class = r.Class,
                            Points = new Points
                            {
                                EffortPoints = currentEffortPoints,
                                EffortPointsDifference = decimal.Round(currentEffortPoints - decayedEffortPoints, MidpointRounding.ToPositiveInfinity),
                                GearPoints = currentGearPoints,
                                GearPointsDifference = decimal.Round(currentGearPoints - decayedGearPoints, MidpointRounding.ToPositiveInfinity),

                            },
                            Active = r.Active
                        };
                    })
                    .OrderByDescending(r => r.Points.Priority)
            };
        }

        public void UpdateEffortPoints(int raiderId, decimal points) => _pointsRepository.UpdateEffortPoints(raiderId, points);

        public void UpdateGearPoints(int raiderId, decimal points) => _pointsRepository.UpdateGearPoints(raiderId, points);

        private DateTime CalculateCutoffDate()
        {
            var today = DateTime.Now;

            var cutOffTime = new TimeSpan(18, 0, 0);
            DayOfWeek cuttOffDayOfWeek;

            switch (today.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    if (today.TimeOfDay < cutOffTime)
                        cuttOffDayOfWeek = DayOfWeek.Wednesday;
                    else
                        cuttOffDayOfWeek = DayOfWeek.Sunday;
                    break;
                case DayOfWeek.Monday:
                case DayOfWeek.Tuesday:
                    cuttOffDayOfWeek = DayOfWeek.Sunday;
                    break;
                case DayOfWeek.Wednesday:
                    if (today.TimeOfDay < cutOffTime)
                        cuttOffDayOfWeek = DayOfWeek.Sunday;
                    else
                        cuttOffDayOfWeek = DayOfWeek.Wednesday;
                    break;
                case DayOfWeek.Thursday:
                case DayOfWeek.Friday:
                case DayOfWeek.Saturday:
                    cuttOffDayOfWeek = DayOfWeek.Wednesday;
                    break;
                default:
                    cuttOffDayOfWeek = DayOfWeek.Sunday;
                    break;
            }

            return PreviousDayOfWeek(today.Date + cutOffTime, cuttOffDayOfWeek);

        }

        private static DateTime PreviousDayOfWeek(DateTime today, DayOfWeek dayOfTheWeek)
        {
            var days = ((int)dayOfTheWeek - (int)today.DayOfWeek - 7) % 7;
            return today.AddDays(days);
        }

        private static Class[] RetrieveClasses(TierToken tierToken)
        {
            switch (tierToken)
            {
                case TierToken.Zenith:
                    return new Class[] { Class.Evoker, Class.Monk, Class.Rogue, Class.Warrior };
                case TierToken.Dreadful:
                    return new Class[] { Class.DeathKnight, Class.DemonHunter, Class.Warlock };
                case TierToken.Mystic:
                    return new Class[] { Class.Druid, Class.Hunter, Class.Mage };
                case TierToken.Venerated:
                    return new Class[] { Class.Paladin, Class.Priest, Class.Shaman };
                default:
                    return Array.Empty<Class>();
            }
        }

        private static Class[] RetrieveClasses(ArmourType armourType)
        {
            switch (armourType)
            {
                case ArmourType.Cloth:
                    return new Class[] { Class.Mage, Class.Priest, Class.Warlock };
                case ArmourType.Leather:
                    return new Class[] { Class.Monk, Class.Rogue, Class.Druid, Class.DemonHunter };
                case ArmourType.Mail:
                    return new Class[] { Class.Evoker, Class.Hunter, Class.Shaman };
                case ArmourType.Plate:
                    return new Class[] { Class.Paladin, Class.DeathKnight, Class.Warrior };
                default:
                    return Array.Empty<Class>();
            }
        }

    }
}
