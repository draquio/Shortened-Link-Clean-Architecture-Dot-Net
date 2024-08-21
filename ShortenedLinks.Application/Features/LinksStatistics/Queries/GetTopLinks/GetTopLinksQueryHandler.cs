
using AutoMapper;
using MediatR;
using ShortenedLinks.Application.DTO.Link;
using ShortenedLinks.Application.DTO.LinkStatistic;
using ShortenedLinks.Application.Interfaces;
using ShortenedLinks.Application.Services.Validation;
using ShortenedLinks.Domain.Entities;
using ShortenedLinks.Domain.Interfaces.Repositories;

namespace ShortenedLinks.Application.Features.LinksStatistics.Queries.GetTopLinks
{
    public class GetTopLinksQueryHandler : IRequestHandler<GetTopLinksQuery, List<LinkClicksStatisticTopDTO>>
    {
        private readonly ILinkStatisticRepository _linkStatisticRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IValidationService _validationService;

        public GetTopLinksQueryHandler(ILinkStatisticRepository linkStatisticRepository, 
            IUserRepository userRepository, 
            IMapper mapper,
            IValidationService validationService)
        {
            _linkStatisticRepository = linkStatisticRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _validationService = validationService;
        }

        public async Task<List<LinkClicksStatisticTopDTO>> Handle(GetTopLinksQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _validationService.IsValidId(request.UserId);
                User user = await _userRepository.GetById(request.UserId);
                if (user == null) throw new KeyNotFoundException($"User with ID {request.UserId} not found");
                List<LinkStatistic> links = await _linkStatisticRepository.GetLinksByRangePeriod(request.UserId, request.PeriodType);
                List<LinkClicksStatisticTopDTO> topLinks = links.GroupBy(stats => stats.LinkId)
                    .Select(group => new LinkClicksStatisticTopDTO
                    {
                        LinkId = group.Key,
                        ClickCount = group.Count(),
                        Link = _mapper.Map<LinkListDTO>(group.FirstOrDefault().Link)
                    })
                    .OrderByDescending(stats => stats.ClickCount)
                    .Take(request.TopN)
                    .ToList();
                return topLinks;
            }
            catch (ArgumentException) { throw; }
            catch (KeyNotFoundException) { throw; }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred:", ex);
            }
        }
    }
}
