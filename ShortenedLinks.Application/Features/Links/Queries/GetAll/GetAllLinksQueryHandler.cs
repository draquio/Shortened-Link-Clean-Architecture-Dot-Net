using AutoMapper;
using MediatR;
using ShortenedLinks.Application.DTO.Link;
using ShortenedLinks.Domain.Entities;
using ShortenedLinks.Domain.Interfaces.Repositories;


namespace ShortenedLinks.Application.Features.Links.Queries.GetAll
{
    public class GetAllLinksQueryHandler : IRequestHandler<GetAllLinksQuery, List<LinkDetailsDTO>>
    {
        private readonly ILinkRepository _linkRepository;
        private readonly IMapper _mapper;

        public GetAllLinksQueryHandler(ILinkRepository linkRepository, IMapper mapper)
        {
            _linkRepository = linkRepository;
            _mapper = mapper;
        }
        public async Task<List<LinkDetailsDTO>> Handle(GetAllLinksQuery request, CancellationToken cancellationToken)
        {
            try
            {
                List<Link> links = await _linkRepository.GetAllWithUsername(request.Page, request.PageSize);
                if (links == null) new List<LinkListDTO>();
                List<LinkDetailsDTO> linkDetailsDTOs = _mapper.Map<List<LinkDetailsDTO>>(links); 
                return linkDetailsDTOs;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while getting the short links.", ex);
            }
        }
    }
}
