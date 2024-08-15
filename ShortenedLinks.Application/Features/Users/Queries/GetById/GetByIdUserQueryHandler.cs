
using AutoMapper;
using MediatR;
using ShortenedLinks.Application.DTO.User;
using ShortenedLinks.Domain.Entities;
using ShortenedLinks.Domain.Interfaces.Repositories;

namespace ShortenedLinks.Application.Features.Users.Queries.GetById
{
    public class GetByIdUserQueryHandler : IRequestHandler<GetByIdUserQuery, UserReadDTO>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetByIdUserQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public async Task<UserReadDTO> Handle(GetByIdUserQuery request, CancellationToken cancellationToken)
        {
            try
            {
                User user = await _userRepository.GetById(request.Id);
                if (user == null) throw new KeyNotFoundException($"User with ID {request.Id} not found");
                UserReadDTO userReadDTO = _mapper.Map<UserReadDTO>(user);
                return userReadDTO;
            }
            catch (KeyNotFoundException) { throw; }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while getting the user.", ex);
            }
        }
    }
}
