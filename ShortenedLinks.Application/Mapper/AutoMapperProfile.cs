using AutoMapper;
using ShortenedLinks.Application.DTO.Link;
using ShortenedLinks.Application.DTO.LinkStatistic;
using ShortenedLinks.Application.DTO.ShortLink;
using ShortenedLinks.Application.DTO.User;
using ShortenedLinks.Domain.Entities;

namespace ShortenedLinks.Application.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            #region User
            CreateMap<User, UserReadDTO>().ReverseMap();
            CreateMap<User, UserCreateDTO>().ReverseMap();
            CreateMap<User, UserUpdateDTO>().ReverseMap();
            #endregion

            #region Link
            CreateMap<Link, LinkListDTO>()
                .ForMember(dto => dto.CreatedAt, options => options.MapFrom(link => link.CreatedAt.ToString("dd/MM/yyyy")));

            CreateMap<Link, LinkDetailsDTO>()
                .ForMember(dto => dto.CreatedAt, options => options.MapFrom(link => link.CreatedAt.ToString("dd/MM/yyyy")))
                .ForMember(dto => dto.Username, options => options.MapFrom(link => link.User.Username));

            CreateMap<Link, ShortLinkDetailDTO>()
                .ForMember(dto => dto.Username, options => options.MapFrom(link => link.User.Username));
            #endregion

        }
    }
}
