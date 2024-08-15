using Microsoft.EntityFrameworkCore;
using ShortenedLinks.Domain.Entities;
using ShortenedLinks.Domain.Interfaces.Repositories;
using ShortenedLinks.Infrastructure.Persistence;

namespace ShortenedLinks.Infrastructure.Repositories
{
    public class LinkRepository : GenericRepository<Link>, ILinkRepository
    {
        public LinkRepository(ShortenedLinksDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> ExistLink(string shortLink)
        {
            try
            {
                bool response = await _dbContext.Set<Link>().AnyAsync(link => link.ShortenedLink == shortLink);
                return response;
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<Link>> GetAllWithUsername(int page, int pageSize)
        {
            try
            {
                List<Link>? links = await _dbContext.Set<Link>()
                    .Include(link => link.User)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
                return links;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Link> GetByShortCode(string shortCode)
        {
            try
            {
                Link? link = await _dbContext.Set<Link>()
                    .Include(link => link.User)
                    .FirstOrDefaultAsync(link => link.ShortenedLink == shortCode);
                return link;
            }
            catch
            {
                throw;
            }
        }
    }
}
