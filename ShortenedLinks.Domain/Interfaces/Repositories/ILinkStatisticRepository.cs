
using ShortenedLinks.Domain.Entities;
using ShortenedLinks.Domain.Enums;

namespace ShortenedLinks.Domain.Interfaces.Repositories
{
    public interface ILinkStatisticRepository : IGenericRepository<LinkStatistic>
    {
        Task<int> CountVisitsByIp(int linkId, string ip);
        Task<List<LinkStatistic>> GetLinksByRangePeriod(int userId, PeriodType period);
        Task<List<LinkStatistic>> GetMonthlyClicks(int userId);
    }
}
