using MediatR;
using ShortenedLinks.Application.DTO.Device;
using ShortenedLinks.Application.Interfaces;
using ShortenedLinks.Application.Services.Validation;
using ShortenedLinks.Domain.Entities;
using ShortenedLinks.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortenedLinks.Application.Features.LinksStatistics.Queries.GetTopDevices
{
    public class GetTopDevicesQueryHandler : IRequestHandler<GetTopDevicesQuery, List<DeviceTopDTO>>
    {
        private readonly ILinkStatisticRepository _linkStatisticRepository;
        private readonly IUserRepository _userRepository;
        private readonly IValidationService _validationService;

        public GetTopDevicesQueryHandler(ILinkStatisticRepository linkStatisticRepository, 
            IUserRepository userRepository,
            IValidationService validationService)
        {
            _linkStatisticRepository = linkStatisticRepository;
            _userRepository = userRepository;
            _validationService = validationService;
        }

        public async Task<List<DeviceTopDTO>> Handle(GetTopDevicesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _validationService.IsValidId(request.UserId);
                User user = await _userRepository.GetById(request.UserId);
                if (user == null) throw new KeyNotFoundException($"User with ID {request.UserId} not found");
                List<LinkStatistic> monthlyclicks = await _linkStatisticRepository.GetMonthlyClicks(request.UserId);
                List<DeviceTopDTO> TopDevices = monthlyclicks.GroupBy(stats => stats.Device)
                    .Select(group => new DeviceTopDTO
                    {
                        DeviceName = group.Key,
                        ClickCount = group.Count()
                    })
                    .OrderByDescending(dto => dto.ClickCount)
                    .ToList();
                return TopDevices;
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
