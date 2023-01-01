using EPGP.API.Responses;
using EPGP.Data.Enums;

namespace EPGP.API.Services
{
    public interface IRaiderIoService
    {
        Task<RaiderIoCharactersProfileResponse> GetCharactersProfile(Region region, string realm, string name);
    }
}
