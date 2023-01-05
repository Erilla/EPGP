using EPGP.API.Models;
using EPGP.API.Responses;
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

        public AllRaiderPointsResponse? GetAllPoints(DateTime? cutOffDate)
        {
            if (!cutOffDate.HasValue) cutOffDate = CalculateCutoffDate();
            var lastUploadedDate = _uploadHistoryRepository.GetLatestUploadDateTime();
            if (lastUploadedDate == null) return null;

            var raiders = _raiderRepository.GetAllRaiders();

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

                        var earliestEffortPoints = effortPoints.Where(p => p.Timestamp > cutOffDate.Value).LastOrDefault()?.Points ?? currentEffortPoints;
                        var earliestGearPoints = gearPoints.Where(p => p.Timestamp > cutOffDate.Value).LastOrDefault()?.Points ?? currentGearPoints;

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
                                EffortPointsDifference = currentEffortPoints - earliestEffortPoints,
                                GearPoints = currentGearPoints,
                                GearPointsDifference = currentGearPoints - earliestGearPoints,

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

    }
}
