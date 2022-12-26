using EPGP.API.Requests;
using Microsoft.AspNetCore.Mvc;

namespace EPGP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadsController : ControllerBase
    {
        [HttpPost]
        public void UploadEPGP([FromBody] UploadEPGPRequest request)
        {
            Console.WriteLine(request);
        }
    }
}
