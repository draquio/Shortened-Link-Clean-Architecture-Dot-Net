
using MediatR;
using ShortenedLinks.Application.DTO.Link;

namespace ShortenedLinks.Application.Features.Links.Commands.Create
{
    public class CreateLinkCommand : IRequest<LinkListDTO>
    {
        public LinkCreateDTO Link { get; set; }
        public CreateLinkCommand(LinkCreateDTO link)
        {
            Link = link;
        }
    }
}
