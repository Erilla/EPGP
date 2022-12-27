using EPGP.API.Requests;
using EPGP.API.Responses;
using EPGP.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace EPGP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadsController : ControllerBase
    {
        private readonly IUploadsService _uploadService;

        public UploadsController(IUploadsService uploadService) => (_uploadService) = (uploadService);

        [HttpPost("epgp")]
        public UploadEPGPResponse UploadEPGP([FromBody] UploadEPGPRequest request)
        {
            return _uploadService.ProcessEPGP(request);
        }
    }
}
