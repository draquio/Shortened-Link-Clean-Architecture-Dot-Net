
using MediatR;
using ShortenedLinks.Application.DTO.ShortLink;

namespace ShortenedLinks.Application.Features.ShortLink.Queries.GetByShortLink
{
    public class GetByShortLinkQuery : IRequest<ShortLinkDetailDTO>
    {
        public string ShortCode { get; set; }

        public GetByShortLinkQuery(string shortCode)
        {
            ShortCode = shortCode;
        }
    }
}
