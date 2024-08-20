using MediatR;
using ShortenedLinks.Application.DTO.Browser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortenedLinks.Application.Features.LinksStatistics.Queries.GetTopBrowsers
{
    public class GetTopBrowsersQuery : IRequest<List<BrowserTopDTO>>
    {
        public int UserId { get; set; }

        public GetTopBrowsersQuery(int userId)
        {
            UserId = userId;
        }
    }
}
