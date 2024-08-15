

using MediatR;
using ShortenedLinks.Application.DTO.User;

namespace ShortenedLinks.Application.Features.Users.Commands.Create
{
    public class CreateUserCommand : IRequest<UserReadDTO>
    {
        public UserCreateDTO User { get; set; }
        public CreateUserCommand(UserCreateDTO user)
        {
            User = user;
        }

    }
}
