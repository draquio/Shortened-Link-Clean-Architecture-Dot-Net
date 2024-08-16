using MediatR;
using ShortenedLinks.Application.DTO.LinkStatistic;
using ShortenedLinks.Domain.Entities;

namespace ShortenedLinks.Application.Features.LinksStatistics.Queries.GetTopLinks
{
    public class GetTopLinksQuery : IRequest<List<LinkClicksStatisticTopDTO>>
    {
        public int UserId { get; set; }
        public PeriodType PeriodType { get; set; }
        public int TopN {  get; set; }
        public GetTopLinksQuery(int userId, PeriodType periodType, int topN)
        {
            UserId = userId;
            PeriodType = periodType;
            TopN = topN;
        }
    }
}
