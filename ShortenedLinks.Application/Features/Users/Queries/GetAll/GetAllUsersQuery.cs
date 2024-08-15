

using MediatR;
using ShortenedLinks.Application.DTO.User;

namespace ShortenedLinks.Application.Features.Users.Queries.GetAll
{
    public class GetAllUsersQuery : IRequest<List<UserReadDTO>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public GetAllUsersQuery(int page, int pageSize)
        {
            Page = page < 1 ? 1 : page;
            PageSize = pageSize < 1 ? 10 : pageSize;
        }
    }
}
