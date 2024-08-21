

using MediatR;
using ShortenedLinks.Application.Interfaces;
using ShortenedLinks.Application.Services.Validation;
using ShortenedLinks.Domain.Entities;
using ShortenedLinks.Domain.Interfaces.Repositories;

namespace ShortenedLinks.Application.Features.LinksStatistics.Commands.Create
{
    public class RegisterLinkStatisticCommandHandler : IRequestHandler<RegisterLinkStatisticCommand, Unit>
    {
        private readonly ILinkStatisticRepository _linkStatisticRepository;
        private readonly IGeoLocationService _geoLocationService;
        private readonly IDeviceInfoService _deviceInfoService;
        private readonly IValidationService _validationService;


        public RegisterLinkStatisticCommandHandler(ILinkStatisticRepository linkStatisticRepository, 
            IGeoLocationService geoLocationService, IDeviceInfoService 
            deviceInfoService,
            IValidationService validationService)
        {
            _linkStatisticRepository = linkStatisticRepository;
            _geoLocationService = geoLocationService;
            _deviceInfoService = deviceInfoService;
            _validationService = validationService;
        }

        public async Task<Unit> Handle(RegisterLinkStatisticCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _validationService.IsValidId(request.LinkId);
                int MaxVisitsPerIp = 3;
                int todayVisits = await _linkStatisticRepository.CountVisitsByIp(request.LinkId, request.VisitorIp);
                if (todayVisits <= MaxVisitsPerIp)
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
                }
                return Unit.Value;
            }
            catch (InvalidOperationException) { throw; }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while setting data.", ex);
            }
        }
    }
}
