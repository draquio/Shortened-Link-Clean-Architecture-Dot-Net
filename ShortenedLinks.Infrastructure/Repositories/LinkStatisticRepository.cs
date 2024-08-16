
using Microsoft.EntityFrameworkCore;
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

        public async Task<int> CountVisitsByIp(int linkId, string ip)
        {
            try
            {
                var today = DateTime.UtcNow.Date;
                int visits = await _dbContext.Set<LinkStatistic>().CountAsync(stats => stats.LinkId == linkId 
                && stats.VisitorIp == ip 
                && stats.VisitDate >= today);
                return visits;
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<LinkClicksStatisticTop>> GetTopLinksByClicks(int userId, PeriodType period, int topN)
        {
            try
            {
                DateTime now = DateTime.Now;
                DateTime startDate;
                DateTime endDate;

                switch (period)
                {
                    case PeriodType.Day:
                        startDate = now.Date;
                        endDate = startDate.AddDays(1).AddTicks(-1);
                        break;
                    case PeriodType.Week:
                        int diff = (7 + (now.DayOfWeek - DayOfWeek.Monday)) % 7;
                        startDate = now.AddDays(-1 * diff).Date;
                        endDate = startDate.AddDays(7).AddTicks(-1);
                        break;
                    case PeriodType.Month:
                        startDate = new DateTime(now.Year, now.Month, 1);
                        endDate = startDate.AddMonths(1).AddTicks(-1);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(period), period, null);
                }
                List<LinkClicksStatisticTop> links = await _dbContext.Set<LinkStatistic>()
                    .Where(stats => stats.Link.UserId == userId && stats.VisitDate >= startDate && stats.VisitDate <= endDate)
                    .GroupBy(stats => stats.LinkId)
                    .Select(g => new LinkClicksStatisticTop
                    {
                        LinkId = g.Key,
                        ClickCount = g.Count(),
                        Link = g.FirstOrDefault().Link,
                        PeriodType = period
                    })
                    .OrderByDescending(stats => stats.ClickCount)
                    .Take(topN)
                    .ToListAsync();
                return links;
            }
            catch
            {
                throw;
            }
        }
    }
}
