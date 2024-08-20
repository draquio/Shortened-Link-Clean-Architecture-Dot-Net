using MediatR;
using ShortenedLinks.Application.DTO.Browser;
using ShortenedLinks.Application.Services.Validation;
using ShortenedLinks.Domain.Entities;
using ShortenedLinks.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortenedLinks.Application.Features.LinksStatistics.Queries.GetTopBrowsers
{
    public class GetTopBrowsersQueryHandler : IRequestHandler<GetTopBrowsersQuery, List<BrowserTopDTO>>
    {
        private readonly ILinkStatisticRepository _linkStatisticRepository;
        private readonly IUserRepository _userRepository;
        private readonly ValidationService _validationService;

        public GetTopBrowsersQueryHandler(ILinkStatisticRepository linkStatisticRepository, 
            IUserRepository userRepository, 
            ValidationService validationService)
        {
            _linkStatisticRepository = linkStatisticRepository;
            _userRepository = userRepository;
            _validationService = validationService;
        }
        public async Task<List<BrowserTopDTO>> Handle(GetTopBrowsersQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _validationService.IsValidId(request.UserId);
                User user = await _userRepository.GetById(request.UserId);
                if (user == null) throw new KeyNotFoundException($"User with ID {request.UserId} not found");
                List<LinkStatistic> mounthlyClicks = await _linkStatisticRepository.GetMonthlyClicks(request.UserId);
                List<BrowserTopDTO> topBrowsers = mounthlyClicks.GroupBy(stats => stats.Browser)
                    .Select(group => new BrowserTopDTO
                    {
                        BrowserName = group.Key,
                        ClickCount = group.Count()
                    })
                    .OrderByDescending(dto => dto.ClickCount)
                    .ToList();
                return topBrowsers;
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
