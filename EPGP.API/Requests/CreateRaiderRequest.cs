using EPGP.Data.Enums;

namespace EPGP.API.Requests
{
    public class CreateRaiderRequest
    {
        public string CharacterName { get; set; }

        public string Realm { get; set; }

        public Region Region { get; set; }

        public Class Class { get; set; }
    }
}
