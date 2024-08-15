using ShortenedLinks.Domain.Entities;

namespace ShortenedLinks.Domain.Interfaces.Repositories
{
    public interface ILinkRepository : IGenericRepository<Link>
    {
        Task<bool> ExistLink(string shortLink);
        Task<List<Link>> GetAllWithUsername(int page, int pageSize);
        Task<Link> GetByShortCode(string shortCode);
    }
}
