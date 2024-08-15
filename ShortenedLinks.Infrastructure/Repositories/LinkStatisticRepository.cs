
using ShortenedLinks.Domain.Entities;
using ShortenedLinks.Domain.Interfaces.Repositories;
using ShortenedLinks.Infrastructure.Persistence;

namespace ShortenedLinks.Infrastructure.Repositories
{
    public class LinkStatisticRepository : GenericRepository<LinkStatistic>, ILinkStatisticRepository
    {
        public LinkStatisticRepository(ShortenedLinksDbContext dbContext) : base(dbContext)
        {
        }
    }
}
