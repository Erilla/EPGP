using EPGP.API.Models;
using EPGP.API.Services;
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
    }
}
