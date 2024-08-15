

using MediatR;
using ShortenedLinks.Application.Interfaces;
using ShortenedLinks.Domain.Entities;
using ShortenedLinks.Domain.Interfaces.Repositories;

namespace ShortenedLinks.Application.Features.LinksStatistics.Commands.Create
{
    public class RegisterLinkStatisticCommandHandler : IRequestHandler<RegisterLinkStatisticCommand, Unit>
    {
        private readonly ILinkStatisticRepository _linkStatisticRepository;
        private readonly IGeoLocationService _geoLocationService;
        private readonly IDeviceInfoService _deviceInfoService;

        public RegisterLinkStatisticCommandHandler(ILinkStatisticRepository linkStatisticRepository,
            IGeoLocationService geoLocationService,
            IDeviceInfoService deviceInfoService)
        {
            _linkStatisticRepository = linkStatisticRepository;
            _geoLocationService = geoLocationService;
            _deviceInfoService = deviceInfoService;
        }

        public async Task<Unit> Handle(RegisterLinkStatisticCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var country = await _geoLocationService.GetCountryByIp(request.VisitorIp);
                var deviceType = _deviceInfoService.GetDeviceType(request.UserAgent);
                LinkStatistic linkStatistics = new LinkStatistic
                {
                    Country = country,
                    Browser = deviceType.Browser,
                    Device = deviceType.Device,
                    LinkId = request.LinkId,
                    VisitDate = DateTime.UtcNow,
                    VisitorIp = request.VisitorIp
                };
                await _linkStatisticRepository.Create(linkStatistics);
                return Unit.Value;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while setting data.", ex);
            }
        }
    }
}
