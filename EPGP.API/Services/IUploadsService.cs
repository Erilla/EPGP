using EPGP.API.Responses;

namespace EPGP.API.Services;

public interface IUploadsService
{
    UploadEPGPResponse ProcessEPGP(string request);
}
