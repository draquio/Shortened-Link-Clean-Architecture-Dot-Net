using MediatR;

namespace ShortenedLinks.Application.Features.Links.Commands.Delete
{
    public class DeleteLinkCommand : IRequest<bool>
    {
        public int Id { get; set; }

        public DeleteLinkCommand(int id)
        {
            Id = id;
        }
    }
}
