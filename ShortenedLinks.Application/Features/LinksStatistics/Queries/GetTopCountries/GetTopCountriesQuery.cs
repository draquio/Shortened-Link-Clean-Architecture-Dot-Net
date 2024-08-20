using MediatR;
using ShortenedLinks.Application.DTO.Country;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortenedLinks.Application.Features.LinksStatistics.Queries.GetTopCountries
{
    public class GetTopCountriesQuery : IRequest<List<CountryTopDTO>>
    {
        public int UserId { get; set; }

        public GetTopCountriesQuery(int userId)
        {
            UserId = userId;
        }
    }
}
