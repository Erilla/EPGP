using EPGP.API.Filters;
using EPGP.API.Models;
using EPGP.API.Responses;
using EPGP.API.Services;
using EPGP.Data.Enums;
using Microsoft.AspNetCore.Mvc;

namespace EPGP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LootController : Controller
    {
        private readonly ILootService _lootService;

        public LootController(ILootService lootService) => (_lootService) = lootService;

        [HttpGet("{raiderId}")]
        public LootHistory Get(int raiderId, int pageSize = 10) => _lootService.GetLootHistory(raiderId, pageSize);

        [HttpGet("region/{region}/realm/{realm}/character/{characterName}")]
        [NotFoundExceptionFilterAttribute]
        public LootHistory GetByCharacterName(Region region, string realm, string characterName, int pageSize = 10) => _lootService.GetLootHistory(region, realm, characterName, pageSize);

        [HttpGet("date/paged/{page}")]
        [NotFoundExceptionFilterAttribute]
        public LootHistoryByDateResponse GetByDate(int page = 0) => _lootService.GetLootHistoryByDate(page);
    }
}
