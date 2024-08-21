

using ShortenedLinks.Application.Interfaces;
using ShortenedLinks.Domain.Interfaces.Repositories;

namespace ShortenedLinks.Application.Services.Links
{
    public class LinkShortenerService : ILinkShortenedService
    {
        private const string Alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private const int ShortLinkLength = 6;

        private readonly ILinkRepository _linkRepository;

        public LinkShortenerService(ILinkRepository linkRepository)
        {
            _linkRepository = linkRepository;
        }
        public async Task<string> GenerateShortLink()
        {
            string shortLink;
            bool exists;

            do
            {
                shortLink = GenerateRandomString(ShortLinkLength);
                exists = await _linkRepository.ExistLink(shortLink);
            }
            while (exists);

            return shortLink;
        }

        private string GenerateRandomString(int length)
        {
            var random = new Random();
            return new string(Enumerable.Repeat(Alphabet, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
