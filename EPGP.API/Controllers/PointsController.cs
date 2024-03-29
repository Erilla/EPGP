﻿using EPGP.API.Filters;
using EPGP.API.Models;
using EPGP.API.Requests;
using EPGP.API.Responses;
using EPGP.API.Services;
using EPGP.Data.Enums;
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
        public Raider Get(int raiderId) => _pointsService.GetPoints(raiderId);

        [HttpGet("raider/all")]
        public AllRaiderPointsResponse? GetAll(DateTime? cutoffDate) => _pointsService.GetAllPoints(cutoffDate);

        [HttpGet("raider/all/tierToken/{tierToken}")]
        public AllRaiderPointsResponse? GetAll(DateTime? cutoffDate, TierToken tierToken) => _pointsService.GetAllPoints(cutoffDate, tierToken: tierToken);

        [HttpGet("raider/all/armour/{armourType}")]
        public AllRaiderPointsResponse? GetAll(DateTime? cutoffDate, ArmourType armourType) => _pointsService.GetAllPoints(cutoffDate, armourType: armourType);

        // POST api/<PointsController>
        [HttpPost("/raider/{raiderId}")]
        [ApiKey]
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
