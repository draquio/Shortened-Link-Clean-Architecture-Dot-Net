using AutoMapper;
using MediatR;
using ShortenedLinks.Application.DTO.LinkStatistic;
using ShortenedLinks.Application.DTO.LinkStatistic.MonthlyClicksByDayDTO;
using ShortenedLinks.Application.Interfaces;
using ShortenedLinks.Application.Services.Validation;
using ShortenedLinks.Domain.Entities;
using ShortenedLinks.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortenedLinks.Application.Features.LinksStatistics.Queries.GetMonthlyClicksByDay
{
    public class GetMonthlyClicksByDayQueryHandler : IRequestHandler<GetMonthlyClicksByDayQuery, MonthlyClicksByDayDTO>
    {
        private readonly ILinkStatisticRepository _linkStatisticRepository;
        private readonly IUserRepository _userRepository;
        private readonly IValidationService _validationService;

        public GetMonthlyClicksByDayQueryHandler(ILinkStatisticRepository linkStatisticRepository, 
            IUserRepository userRepository,
            IValidationService validationService)
        {
            _linkStatisticRepository = linkStatisticRepository;
            _userRepository = userRepository;
            _validationService = validationService;
        }

        public async Task<MonthlyClicksByDayDTO> Handle(GetMonthlyClicksByDayQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _validationService.IsValidId(request.UserId);
                User user = await _userRepository.GetById(request.UserId);
                if (user == null) throw new KeyNotFoundException($"User with ID {request.UserId} not found");
                List<LinkStatistic> monthlyclicks = await _linkStatisticRepository.GetMonthlyClicks(request.UserId);
                var Monthlystats = new MonthlyClicksByDayDTO
                {
                    monthlyClicksByDay = monthlyclicks.GroupBy(stats => stats.VisitDate.Date)
                    .Select(group => new RangedClicksDTO
                    {
                        DateClicks = group.Key.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                        Clicks = group.Count()
                    }).ToList(),
                    userId = user.Id
                };

                return Monthlystats;
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
