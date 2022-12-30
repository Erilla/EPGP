using EPGP.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace EPGP.API.Controllers
{
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
    }
}
