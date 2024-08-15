

using MediatR;

namespace ShortenedLinks.Application.Features.Users.Commands.Delete
{
    public class DeleteUserCommand : IRequest<bool>
    {
        public int Id { get; set; }

        public DeleteUserCommand(int id)
        {
            Id = id;
        }
    }
}
