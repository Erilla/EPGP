using EPGP.API.Filters;
using EPGP.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace EPGP.API.Controllers
{
    [ApiKey]
    public class AdminController : Controller
    {

        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService) => (_adminService) = (adminService);

        [HttpPost("CreateDatabase")]
        public IActionResult CreateDatabase()
        {
            _adminService.CreateDatabases();
            return Ok();
        }

        [HttpPost("DeleteDatabase")]
        public IActionResult DeleteDatabase()
        {
            _adminService.DeleteDatabases();
            return Ok();
        }

        [HttpPost("RecreateDatabase")]
        public IActionResult RecreateDatabase()
        {
            _adminService.DeleteDatabases();
            _adminService.CreateDatabases();
            return Ok();
        }

        [HttpPost("FillRaiderDetails")]
        public async Task<IActionResult> FillRaiderDetails()
        {
            await _adminService.FillRaiderDetails();
            return Ok();
        }
    }
}
