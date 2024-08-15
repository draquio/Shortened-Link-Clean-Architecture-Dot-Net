

using MediatR;
using ShortenedLinks.Application.DTO.Link;

namespace ShortenedLinks.Application.Features.Links.Queries.GetById
{
    public class GetByIdLinkQuery : IRequest<LinkDetailsDTO>
    {
        public int Id { get; set; }

        public GetByIdLinkQuery(int id)
        {
            Id = id;
        }
    }
}
