using MediatR;
using ShortenedLinks.Application.DTO.Country;
using ShortenedLinks.Application.Services.Validation;
using ShortenedLinks.Domain.Entities;
using ShortenedLinks.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortenedLinks.Application.Features.LinksStatistics.Queries.GetTopCountries
{
    public class GetTopCountriesQueryHandler : IRequestHandler<GetTopCountriesQuery, List<CountryTopDTO>>
    {
        private readonly ILinkStatisticRepository _linkStatisticRepository;
        private readonly IUserRepository _userRepository;
        private readonly ValidationService _validationService;

        public GetTopCountriesQueryHandler(ILinkStatisticRepository linkStatisticRepository, IUserRepository userRepository, ValidationService validationService)
        {
            _linkStatisticRepository = linkStatisticRepository;
            _userRepository = userRepository;
            _validationService = validationService;
        }
        public async Task<List<CountryTopDTO>> Handle(GetTopCountriesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _validationService.IsValidId(request.UserId);
                User user = await _userRepository.GetById(request.UserId);
                if (user == null) throw new KeyNotFoundException($"User with ID {request.UserId} not found");
                List<LinkStatistic> mounthlyClicks = await _linkStatisticRepository.GetMonthlyClicks(user.Id);
                List<CountryTopDTO> topCountries = mounthlyClicks.GroupBy(stats => stats.Country)
                    .Select(group => new CountryTopDTO
                    {
                        CountryName = group.Key,
                        ClickCount = group.Count()
                    })
                    .OrderByDescending(dto => dto.ClickCount)
                    .ToList();
                return topCountries;
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
