using MediatR;
using ShortenedLinks.Application.DTO.User;

namespace ShortenedLinks.Application.Features.Users.Queries.GetById
{
    public class GetByIdUserQuery : IRequest<UserReadDTO>
    {
        public int Id { get; set; }

        public GetByIdUserQuery(int id)
        {
            Id = id < 1 ? 1 : id;
        }
    }
}
