using EPGP.API.Requests;
using EPGP.API.Responses;

namespace EPGP.API.Services;

public interface IUploadsService
{
    UploadEPGPResponse ProcessEPGP(UploadEPGPRequest request);
}
