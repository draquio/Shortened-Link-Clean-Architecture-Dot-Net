

using AutoMapper;
using MediatR;
using ShortenedLinks.Application.DTO.User;
using ShortenedLinks.Domain.Entities;
using ShortenedLinks.Domain.Interfaces.Repositories;

namespace ShortenedLinks.Application.Features.Users.Commands.Create
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserReadDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public CreateUserCommandHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public async Task<UserReadDTO> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                User user = _mapper.Map<User>(request.User);
                User userCreated = await _userRepository.Create(user);
                UserReadDTO userReadDTO = _mapper.Map<UserReadDTO>(user);
                return userReadDTO;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while creating the user.", ex);
            }
        }
    }
}
