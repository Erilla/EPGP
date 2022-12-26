using EPGP.API.Requests;

namespace EPGP.API.Services;

public interface IUploadsService
{
    void ProcessEPGP(UploadEPGPRequest request);
}
