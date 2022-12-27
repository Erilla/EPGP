using EPGP.API.Requests;
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
        public void UploadEPGP([FromBody] UploadEPGPRequest request)
        {
            _uploadService.ProcessEPGP(request);
        }
    }
}
