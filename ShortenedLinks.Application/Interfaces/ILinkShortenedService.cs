

namespace ShortenedLinks.Application.Interfaces
{
    public interface ILinkShortenedService
    {
        Task<string> GenerateShortLink();
    }
}
