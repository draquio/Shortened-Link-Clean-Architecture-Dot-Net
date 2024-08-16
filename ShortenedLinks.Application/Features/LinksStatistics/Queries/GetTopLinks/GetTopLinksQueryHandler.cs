
using AutoMapper;
using MediatR;
using ShortenedLinks.Application.DTO.LinkStatistic;
using ShortenedLinks.Application.Services.Validation;
using ShortenedLinks.Domain.Entities;
using ShortenedLinks.Domain.Interfaces.Repositories;

namespace ShortenedLinks.Application.Features.LinksStatistics.Queries.GetTopLinks
{
    public class GetTopLinksQueryHandler : IRequestHandler<GetTopLinksQuery, List<LinkClicksStatisticTopDTO>>
    {
        private readonly ILinkStatisticRepository _linkStatisticRepository;
        private readonly IMapper _mapper;
        private readonly ValidationService _validationService;

        public GetTopLinksQueryHandler(ILinkStatisticRepository linkStatisticRepository, IMapper mapper, ValidationService validationService)
        {
            _linkStatisticRepository = linkStatisticRepository;
            _mapper = mapper;
            _validationService = validationService;
        }

        public async Task<List<LinkClicksStatisticTopDTO>> Handle(GetTopLinksQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _validationService.IsValidId(request.UserId);
                List<LinkClicksStatisticTop> topLinks = await _linkStatisticRepository.GetTopLinksByClicks(request.UserId, request.PeriodType, request.TopN);
                List<LinkClicksStatisticTopDTO> topLinksDTO = _mapper.Map<List<LinkClicksStatisticTopDTO>>(topLinks);
                return topLinksDTO;
            }
            catch (ArgumentException) { throw; }
            catch (KeyNotFoundException) { throw; }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while getting the short link.", ex);
            }
        }
    }
}
