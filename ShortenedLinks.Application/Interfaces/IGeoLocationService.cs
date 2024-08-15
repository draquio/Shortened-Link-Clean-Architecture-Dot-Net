
namespace ShortenedLinks.Application.Interfaces
{
    public interface IGeoLocationService
    {
        Task<string> GetCountryByIp(string ip);
    }
}
