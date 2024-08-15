

using MediatR;
using ShortenedLinks.Application.DTO.Link;

namespace ShortenedLinks.Application.Features.Links.Queries.GetAll
{
    public class GetAllLinksQuery : IRequest<List<LinkDetailsDTO>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public GetAllLinksQuery(int page, int pageSize)
        {
            Page = page < 1 ? 1 : page;
            PageSize = pageSize < 1 ? 10 : pageSize;
        }

    }
}
