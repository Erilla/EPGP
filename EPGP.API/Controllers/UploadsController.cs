using EPGP.API.Filters;
using EPGP.API.Responses;
using EPGP.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace EPGP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiKey]
    public class UploadsController : ControllerBase
    {
        private readonly IUploadsService _uploadService;

        public UploadsController(IUploadsService uploadService) => (_uploadService) = (uploadService);

        [HttpPost("epgp")]
        public UploadEPGPResponse UploadEPGP([FromBody] JsonElement request)
        {
            string json = JsonSerializer.Serialize(request);
            return _uploadService.ProcessEPGP(json);
        }
    }
}
