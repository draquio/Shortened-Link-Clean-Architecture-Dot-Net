using Moq;
using ShortenedLinks.Application.Features.LinksStatistics.Commands.Create;
using ShortenedLinks.Application.Interfaces;
using ShortenedLinks.Application.Services.DeviceInfo;
using ShortenedLinks.Domain.Entities;
using ShortenedLinks.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortenedLinks.Tests.ApplicationTest.Features.LinksStatisticTest.Commands
{
    public class RegisterLinkStatisticCommandTests
    {
        private readonly Mock<ILinkStatisticRepository> _mockLinkStatisticRepository;
        private readonly Mock<IGeoLocationService> _mockGeoLocationService;
        private readonly Mock<IDeviceInfoService> _mockDeviceInfoService;
        private readonly Mock<IValidationService> _mockValidationService;
        private readonly RegisterLinkStatisticCommandHandler _handler;
        int linkId = 1, countsByIp = 3;
        string geoCountry= "US";
        RegisterLinkStatisticCommand command = new RegisterLinkStatisticCommand(1, "192.168.1.1", "Mozilla/5.0");
        public RegisterLinkStatisticCommandTests()
        {
            _mockLinkStatisticRepository = new Mock<ILinkStatisticRepository>();
            _mockGeoLocationService = new Mock<IGeoLocationService>();
            _mockDeviceInfoService = new Mock<IDeviceInfoService>();
            _mockValidationService = new Mock<IValidationService>();
            _handler = new RegisterLinkStatisticCommandHandler(
                _mockLinkStatisticRepository.Object,
                _mockGeoLocationService.Object,
                _mockDeviceInfoService.Object,
                _mockValidationService.Object
            );
        }
        [Fact]
        public async Task Handle_ShouldRegisterVisit_WhenVisitsAreBelowThreshold()
        {
            _mockValidationService.Setup(v => v.IsValidId(command.LinkId));
            _mockLinkStatisticRepository.Setup(repo => repo.CountVisitsByIp(command.LinkId, command.VisitorIp))
                .ReturnsAsync(countsByIp);
            _mockGeoLocationService.Setup(geo => geo.GetCountryByIp(command.VisitorIp))
                .ReturnsAsync(geoCountry);
            _mockDeviceInfoService.Setup(device => device.GetDeviceType(command.UserAgent))
                .Returns(new DeviceInfo { Device = "Mobile", Browser = "Chrome" });

            await _handler.Handle(command, CancellationToken.None);

            _mockLinkStatisticRepository.Verify(repo => repo.Create(It.IsAny<LinkStatistic>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldNotRegisterVisit_WhenVisitsExceedThreshold()
        {
            _mockValidationService.Setup(v => v.IsValidId(command.LinkId));
            _mockLinkStatisticRepository.Setup(repo => repo.CountVisitsByIp(command.LinkId, command.VisitorIp))
                .ReturnsAsync(4);

            await _handler.Handle(command, CancellationToken.None);

            _mockLinkStatisticRepository.Verify(repo => repo.Create(It.IsAny<LinkStatistic>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldThrowApplicationException_WhenExceptionOccurs()
        {
            _mockValidationService.Setup(v => v.IsValidId(command.LinkId));
            _mockLinkStatisticRepository.Setup(repo => repo.CountVisitsByIp(command.LinkId, command.VisitorIp))
                .ThrowsAsync(new Exception("Database error"));

            await Assert.ThrowsAsync<ApplicationException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }
    }
}
