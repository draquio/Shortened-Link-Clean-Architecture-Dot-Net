using ShortenedLinks.Application.Interfaces;


namespace ShortenedLinks.Application.Services.GeoLocation
{
    public class GeoLocationService : IGeoLocationService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public GeoLocationService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> GetCountryByIp(string ip)
        {
            using (var httpClient = _httpClientFactory.CreateClient())
            {
                if (ip == "::1") ip = "8.8.8.8";
                var response = await httpClient.GetStringAsync($"https://ipinfo.io/{ip}/country");
                return response.Trim();
            }
        }
    }
}
