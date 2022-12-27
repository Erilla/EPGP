using EPGP.API.Requests;
using EPGP.API.Responses;
using EPGP.Data.Enums;
using Hangfire;

namespace EPGP.API.Services
{
    public class UploadsService : IUploadsService
    {
        private readonly IRaiderService _raiderService;
        private readonly IPointsService _pointsService;
        private readonly IBackgroundJobClient _backgroundJobs;

        public UploadsService(IRaiderService raiderService, IPointsService pointsService, IBackgroundJobClient backgroundJobs) =>
            (_raiderService, _pointsService, _backgroundJobs) = (raiderService, pointsService, backgroundJobs);

        public UploadEPGPResponse ProcessEPGP(UploadEPGPRequest request)
        {
            var region = string.IsNullOrEmpty(request.Region) ? Region.Unknown : (Region)Enum.Parse(typeof(Region), request.Region);

            var rosterJobId = _backgroundJobs.Enqueue(() => ProcessRoster(region, ConvertRosterObjectToClass(request.Roster)));

            return new UploadEPGPResponse
            {
                RosterJobId = rosterJobId
            };
        }

        public void ProcessRoster(Region region, IEnumerable<UploadEPGPRoster> roster)
        {
            foreach (var raider in roster)
            {
                var raiderId = _raiderService.CreateRaider(new Models.Raider
                {
                    CharacterName = raider.CharacterName,
                    Realm = raider.Realm,
                    Region = region,
                });

                _pointsService.UpdateEffortPoints(raiderId, raider.EffortPoints);
                _pointsService.UpdateGearPoints(raiderId, raider.GearPoints);
            }
        }

        private IEnumerable<UploadEPGPRoster> ConvertRosterObjectToClass(object[][] roster)
        {
            var classObject = new List<UploadEPGPRoster>();

            foreach (var raider in roster)
            {
                var character = raider[0].ToString();

                var characterName = character.Split("-")[0];
                var characterRealm = character.Split("-")[1];

                var effortPointsStringObject = raider[1].ToString() ?? string.Empty;
                var gearPointsStringObject = raider[2].ToString() ?? string.Empty;

                var effortPoints = string.IsNullOrEmpty(effortPointsStringObject) ? 0 : int.Parse(effortPointsStringObject);
                var gearPoints = string.IsNullOrEmpty(gearPointsStringObject) ? 0 : int.Parse(gearPointsStringObject);

                classObject.Add(new UploadEPGPRoster
                {
                    CharacterName = characterName,
                    Realm = characterRealm,
                    EffortPoints = effortPoints,
                    GearPoints = gearPoints
                });
            }

            return classObject;
        }
    }
}
