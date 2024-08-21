using Moq;
using ShortenedLinks.Application.Features.LinksStatistics.Queries.GetTopDevices;
using ShortenedLinks.Application.Interfaces;
using ShortenedLinks.Domain.Entities;
using ShortenedLinks.Domain.Interfaces.Repositories;


namespace ShortenedLinks.Tests.ApplicationTest.Features.LinksStatisticTest.Queries
{
    public class GetTopDevicesQueryTests
    {
        private readonly Mock<ILinkStatisticRepository> _mockLinkStatisticRepository;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IValidationService> _mockValidationService;
        private readonly GetTopDevicesQueryHandler _handler;
        int userId = 1;
        User user = new User { Id = 1 };
        List<LinkStatistic> linkStatistics = new List<LinkStatistic>
        {
            new LinkStatistic { Device = "Mobile" },
            new LinkStatistic { Device = "Desktop" },
            new LinkStatistic { Device = "Mobile" }
        };
        public GetTopDevicesQueryTests()
        {
            _mockLinkStatisticRepository = new Mock<ILinkStatisticRepository>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockValidationService = new Mock<IValidationService>();
            _handler = new GetTopDevicesQueryHandler(
                _mockLinkStatisticRepository.Object,
                _mockUserRepository.Object,
                _mockValidationService.Object
            );
        }
        [Fact]
        public async Task Handle_ReturnsTopDevices_WhenUserExists()
        {
            var query = new GetTopDevicesQuery(userId);
            _mockValidationService.Setup(v => v.IsValidId(query.UserId));
            _mockUserRepository.Setup(repo => repo.GetById(query.UserId)).ReturnsAsync(user);
            _mockLinkStatisticRepository.Setup(repo => repo.GetMonthlyClicks(query.UserId)).ReturnsAsync(linkStatistics);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Mobile", result[0].DeviceName);
            Assert.Equal(2, result[0].ClickCount);
            Assert.Equal("Desktop", result[1].DeviceName);
            Assert.Equal(1, result[1].ClickCount);
        }

        [Fact]
        public async Task Handle_ThrowsKeyNotFoundException_WhenUserDoesNotExist()
        {
            var query = new GetTopDevicesQuery(userId);
            _mockValidationService.Setup(v => v.IsValidId(query.UserId));
            _mockUserRepository.Setup(repo => repo.GetById(query.UserId)).ReturnsAsync((User)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _handler.Handle(query, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ThrowsApplicationException_WhenExceptionOccurs()
        {
            var query = new GetTopDevicesQuery(userId);
            _mockValidationService.Setup(v => v.IsValidId(query.UserId));
            _mockUserRepository.Setup(repo => repo.GetById(query.UserId)).ReturnsAsync(user);
            _mockLinkStatisticRepository.Setup(repo => repo.GetMonthlyClicks(query.UserId))
                .ThrowsAsync(new Exception("Simulated exception"));

            await Assert.ThrowsAsync<ApplicationException>(() =>
                _handler.Handle(query, CancellationToken.None));
        }
    }
}
