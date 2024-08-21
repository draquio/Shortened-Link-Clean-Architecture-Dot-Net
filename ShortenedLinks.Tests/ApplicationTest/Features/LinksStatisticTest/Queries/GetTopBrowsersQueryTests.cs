using Moq;
using ShortenedLinks.Application.Features.LinksStatistics.Queries.GetTopBrowsers;
using ShortenedLinks.Application.Interfaces;
using ShortenedLinks.Domain.Entities;
using ShortenedLinks.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortenedLinks.Tests.ApplicationTest.Features.LinksStatisticTest.Queries
{
    public class GetTopBrowsersQueryTests
    {
        private readonly Mock<ILinkStatisticRepository> _mockLinkStatisticRepository;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IValidationService> _mockValidationService;
        private readonly GetTopBrowsersQueryHandler _handler;
        int userId = 1;
        User user = new User { Id = 1 };
        List<LinkStatistic> linkStatistics = new List<LinkStatistic>
        {
            new LinkStatistic { Browser = "Chrome" },
            new LinkStatistic { Browser = "Firefox" },
            new LinkStatistic { Browser = "Chrome" }
        };
        public GetTopBrowsersQueryTests()
        {
            _mockLinkStatisticRepository = new Mock<ILinkStatisticRepository>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockValidationService = new Mock<IValidationService>();
            _handler = new GetTopBrowsersQueryHandler(
                _mockLinkStatisticRepository.Object,
                _mockUserRepository.Object,
                _mockValidationService.Object
            );
        }
        [Fact]
        public async Task Handle_ReturnsTopBrowsers_WhenUserExists()
        {
            var query = new GetTopBrowsersQuery(userId);
            _mockValidationService.Setup(v => v.IsValidId(query.UserId));
            _mockUserRepository.Setup(repo => repo.GetById(query.UserId)).ReturnsAsync(user);
            _mockLinkStatisticRepository.Setup(repo => repo.GetMonthlyClicks(query.UserId)).ReturnsAsync(linkStatistics);
            var result = await _handler.Handle(query, CancellationToken.None);
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Chrome", result[0].BrowserName);
            Assert.Equal(2, result[0].ClickCount);
            Assert.Equal("Firefox", result[1].BrowserName);
            Assert.Equal(1, result[1].ClickCount);
        }

        [Fact]
        public async Task Handle_ThrowsKeyNotFoundException_WhenUserDoesNotExist()
        {
            var query = new GetTopBrowsersQuery(userId);
            _mockValidationService.Setup(v => v.IsValidId(query.UserId));
            _mockUserRepository.Setup(repo => repo.GetById(query.UserId)).ReturnsAsync((User)null);
            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _handler.Handle(query, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ThrowsApplicationException_WhenExceptionOccurs()
        {
            var query = new GetTopBrowsersQuery(userId);
            _mockValidationService.Setup(v => v.IsValidId(query.UserId));
            _mockUserRepository.Setup(repo => repo.GetById(query.UserId)).ReturnsAsync(user);
            _mockLinkStatisticRepository.Setup(repo => repo.GetMonthlyClicks(query.UserId))
                .ThrowsAsync(new Exception("Simulated exception"));

            await Assert.ThrowsAsync<ApplicationException>(() =>
                _handler.Handle(query, CancellationToken.None));
        }
    }
}
