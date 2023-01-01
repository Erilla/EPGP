using EPGP.API.Responses;
using EPGP.Data.Enums;
using System.Text.Json;

namespace EPGP.API.Services
{
    public class RaiderIoService : IRaiderIoService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public RaiderIoService(IHttpClientFactory httpClientFactory) => _httpClientFactory = httpClientFactory;

        public async Task<RaiderIoCharactersProfileResponse> GetCharactersProfile(Region region, string realm, string name)
        {
            var charactersProfileUrl = $"https://raider.io/api/v1/characters/profile?region={region}&realm={realm}&name={name}";
            var httpClient = _httpClientFactory.CreateClient();
            var httpResponse = await httpClient.GetAsync(charactersProfileUrl);

            if (httpResponse.IsSuccessStatusCode)
            {
                using var contentStream =
                    await httpResponse.Content.ReadAsStreamAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                var test = await JsonSerializer.DeserializeAsync
                    <RaiderIoCharactersProfileResponse>(contentStream, options);

                return test ?? new RaiderIoCharactersProfileResponse();
            }

            return new RaiderIoCharactersProfileResponse();
        }
    }
}
