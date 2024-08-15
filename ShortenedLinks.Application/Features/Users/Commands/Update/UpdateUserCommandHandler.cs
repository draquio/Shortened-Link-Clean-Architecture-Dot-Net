using AutoMapper;
using MediatR;
using ShortenedLinks.Application.DTO.User;
using ShortenedLinks.Domain.Entities;
using ShortenedLinks.Domain.Interfaces.Repositories;


namespace ShortenedLinks.Application.Features.Users.Commands.Update
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UpdateUserCommandHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                User user = await _userRepository.GetById(request.User.Id);
                if(user == null) throw new KeyNotFoundException($"User with ID {request.User.Id} not found");
                _mapper.Map(request.User, user);
                bool response = await _userRepository.Update(user);
                if (!response) throw new InvalidOperationException("User could not be updated");
                return response;
            }
            catch (KeyNotFoundException) { throw; }
            catch (InvalidOperationException) { throw; }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while updating the user.", ex);
            }
        }
    }
}
