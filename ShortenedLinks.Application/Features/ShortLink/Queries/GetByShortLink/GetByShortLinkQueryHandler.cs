
using AutoMapper;
using MediatR;
using ShortenedLinks.Application.DTO.ShortLink;
using ShortenedLinks.Application.Services.Validation;
using ShortenedLinks.Domain.Entities;
using ShortenedLinks.Domain.Interfaces.Repositories;

namespace ShortenedLinks.Application.Features.ShortLink.Queries.GetByShortLink
{
    public class GetByShortLinkQueryHandler : IRequestHandler<GetByShortLinkQuery, ShortLinkDetailDTO>
    {
        private readonly ILinkRepository _linkRepository;
        private readonly IMapper _mapper;
        public GetByShortLinkQueryHandler(ILinkRepository linkRepository, IMapper mapper)
        {
            _linkRepository = linkRepository;
            _mapper = mapper;
        }

        public async Task<ShortLinkDetailDTO> Handle(GetByShortLinkQuery request, CancellationToken cancellationToken)
        {
            try
            {
                Link link = await _linkRepository.GetByShortCode(request.ShortCode);
                if(link == null) throw new KeyNotFoundException($"Post with Short Code {request.ShortCode} not found");
                ShortLinkDetailDTO shortLinkDetailDTO = _mapper.Map<ShortLinkDetailDTO>(link);
                return shortLinkDetailDTO;
            }
            catch (KeyNotFoundException) { throw; }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while getting the short link.", ex);
            }
        }
    }
}
