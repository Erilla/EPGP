using EPGP.API.Requests;
using EPGP.Data.Enums;

namespace EPGP.API.Services
{
    public class UploadsService : IUploadsService
    {
        private readonly IRaiderService _raiderService;
        private readonly IPointsService _pointsService;

        public UploadsService(IRaiderService raiderService, IPointsService pointsService) => (_raiderService, _pointsService) = (raiderService, pointsService);

        public void ProcessEPGP(UploadEPGPRequest request)
        {
            var region = string.IsNullOrEmpty(request.Region) ? Region.Unknown : (Region)Enum.Parse(typeof(Region), request.Region);

            ProcessRoster(region, request.Roster);
        }

        private void ProcessRoster(Region region, object[][] roster)
        {
            foreach (var raider in roster)
            {
                var character = raider[0].ToString();

                var characterName = character.Split("-")[0];
                var characterRealm = character.Split("-")[1];

                var effortPointsStringObject = raider[1].ToString() ?? string.Empty;
                var gearPointsStringObject = raider[2].ToString() ?? string.Empty;

                var effortPoints = string.IsNullOrEmpty(effortPointsStringObject) ? 0 : int.Parse(effortPointsStringObject);
                var gearPoints = string.IsNullOrEmpty(gearPointsStringObject) ? 0 : int.Parse(gearPointsStringObject);

                var raiderId = _raiderService.CreateRaider(new Models.Raider
                {
                    CharacterName = characterName,
                    Realm = characterRealm,
                    Region = region,
                });

                _pointsService.UpdateEffortPoints(raiderId, effortPoints);
                _pointsService.UpdateGearPoints(raiderId, gearPoints);
            }
        }
    }
}
