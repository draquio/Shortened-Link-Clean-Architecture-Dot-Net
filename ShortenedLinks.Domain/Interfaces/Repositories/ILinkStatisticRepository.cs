
using ShortenedLinks.Domain.Entities;

namespace ShortenedLinks.Domain.Interfaces.Repositories
{
    public interface ILinkStatisticRepository : IGenericRepository<LinkStatistic>
    {
        Task<int> CountVisitsByIp(int linkId, string ip);
        Task<List<LinkClicksStatisticTop>> GetTopLinksByClicks(int userId, PeriodType period, int topN);
    }
}
