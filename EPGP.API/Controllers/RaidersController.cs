using EPGP.API.Models;
using EPGP.API.Requests;
using EPGP.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace EPGP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RaidersController : ControllerBase
    {
        private readonly IRaiderService _raiderService;

        public RaidersController(IRaiderService raiderService) => (_raiderService) = (raiderService);


        [HttpGet("{raiderId}")]
        public Raider Get(int raiderId) => _raiderService.GetRaider(raiderId);

        [HttpPost]
        public void Post([FromBody] CreateRaiderRequest request)
        {
            _raiderService.CreateRaider(new Raider
            {
                CharacterName = request.CharacterName,
                Class = request.Class,
                Realm = request.Realm,
                Region = request.Region,
            });
        }
    }
}
