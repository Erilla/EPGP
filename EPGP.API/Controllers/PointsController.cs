using EPGP.API.Models;
using EPGP.API.Requests;
using EPGP.API.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EPGP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PointsController : ControllerBase
    {
        private readonly IPointsService _pointsService;
        public PointsController(IPointsService pointsService) => (_pointsService) = (pointsService);

        // GET api/<PointsController>/5
        [HttpGet("raider/{raiderId}")]
        public Points Get(int raiderId) => _pointsService.GetPoints(raiderId);

        [HttpGet("raider/All")]
        public IEnumerable<Points> GetAll() => _pointsService.GetAllPoints();

        // POST api/<PointsController>
        [HttpPost("/raider/{raiderId}")]
        public IActionResult Post(int raiderId, [FromBody] UpdatePointsRequest request)
        {
            if (!request.EffortPoints.HasValue && !request.GearPoints.HasValue)
            {
                return BadRequest("No Effort Points or Gear Points set");
            }

            if (request.EffortPoints.HasValue)
            {
                _pointsService.UpdateEffortPoints(raiderId, request.EffortPoints.Value);
            }

            if (request.GearPoints.HasValue)
            {
                _pointsService.UpdateGearPoints(raiderId, request.GearPoints.Value);
            }

            return Ok();
        }
    }
}
