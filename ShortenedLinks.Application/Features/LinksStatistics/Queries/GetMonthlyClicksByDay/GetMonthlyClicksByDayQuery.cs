using MediatR;
using ShortenedLinks.Application.DTO.LinkStatistic;
using ShortenedLinks.Application.DTO.LinkStatistic.MonthlyClicksByDayDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortenedLinks.Application.Features.LinksStatistics.Queries.GetMonthlyClicksByDay
{
    public class GetMonthlyClicksByDayQuery : IRequest<MonthlyClicksByDayDTO>
    {
        public int UserId { get; set; }
        public GetMonthlyClicksByDayQuery(int userId)
        {
            UserId = userId;
        }
    }
}
