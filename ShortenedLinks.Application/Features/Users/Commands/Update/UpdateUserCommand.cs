
using MediatR;
using ShortenedLinks.Application.DTO.User;

namespace ShortenedLinks.Application.Features.Users.Commands.Update
{
    public class UpdateUserCommand : IRequest<bool>
    {
        public UserUpdateDTO User { get; set; }

        public UpdateUserCommand(UserUpdateDTO user)
        {
            User = user;
        }
    }
}
