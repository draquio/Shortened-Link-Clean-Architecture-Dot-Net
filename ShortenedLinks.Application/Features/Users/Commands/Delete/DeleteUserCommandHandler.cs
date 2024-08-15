

using MediatR;
using ShortenedLinks.Domain.Entities;
using ShortenedLinks.Domain.Interfaces.Repositories;

namespace ShortenedLinks.Application.Features.Users.Commands.Delete
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
    {
        private readonly IUserRepository _userRepository;

        public DeleteUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                User user = await _userRepository.GetById(request.Id);
                if (user == null) throw new KeyNotFoundException($"User with ID {request.Id} not found");
                bool response = await _userRepository.Delete(user);
                if (!response) throw new InvalidOperationException("User could not be updated");
                return response;
            }
            catch (KeyNotFoundException) { throw; }
            catch (ArgumentException) { throw; }
            catch (InvalidOperationException) { throw; }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while deleting the user.", ex);
            }
        }
    }
}
