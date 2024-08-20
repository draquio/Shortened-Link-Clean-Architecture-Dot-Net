
using Microsoft.EntityFrameworkCore;
using ShortenedLinks.Domain.Entities;
using ShortenedLinks.Domain.Enums;
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
        public async Task<List<LinkStatistic>> GetMonthlyClicks(int userId)
        {
            try
            {
                var startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                var endDate = startDate.AddMonths(1).AddDays(-1);
                List<LinkStatistic> monthlyClicks = await _dbContext.Set<LinkStatistic>()
                     .Where(stat => stat.Link.UserId == userId && stat.VisitDate >= startDate && stat.VisitDate <= endDate)
                     .ToListAsync();
                return monthlyClicks;
            }
            catch
            {
                throw;
            }
        }
        public async Task<List<LinkStatistic>> GetLinksByRangePeriod(int userId, PeriodType period)
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
                List<LinkStatistic> links = await _dbContext.Set<LinkStatistic>()
                    .Where(stats => stats.Link.UserId == userId && stats.VisitDate >= startDate && stats.VisitDate <= endDate)
                    .Include(link => link.Link)
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
