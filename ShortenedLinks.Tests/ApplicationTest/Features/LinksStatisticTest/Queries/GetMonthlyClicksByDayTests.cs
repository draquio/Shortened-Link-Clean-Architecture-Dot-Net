using Moq;
using ShortenedLinks.Application.Features.LinksStatistics.Queries.GetMonthlyClicksByDay;
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
    public class GetMonthlyClicksByDayTests
    {
        private readonly Mock<ILinkStatisticRepository> _mockLinkStatisticRepository;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IValidationService> _mockValidationService;
        private readonly GetMonthlyClicksByDayQueryHandler _handler;
        int userId = 1;
        User user = new User { Id = 1 };
        List<LinkStatistic> linkStatistics = new List<LinkStatistic>
        {
            new LinkStatistic { VisitDate = DateTime.UtcNow.Date },
            new LinkStatistic { VisitDate = DateTime.UtcNow.Date.AddDays(-1) },
        };
        public GetMonthlyClicksByDayTests()
        {
            _mockLinkStatisticRepository = new Mock<ILinkStatisticRepository>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockValidationService = new Mock<IValidationService>();
            _handler = new GetMonthlyClicksByDayQueryHandler(
                _mockLinkStatisticRepository.Object,
                _mockUserRepository.Object,
                _mockValidationService.Object
            );
        }
        [Fact]
        public async Task Handle_ReturnsMonthlyClicksByDay_WhenUserExists()
        {
            var query = new GetMonthlyClicksByDayQuery(userId);
            _mockValidationService.Setup(v => v.IsValidId(query.UserId));
            _mockUserRepository.Setup(repo => repo.GetById(query.UserId)).ReturnsAsync(user);
            _mockLinkStatisticRepository.Setup(repo => repo.GetMonthlyClicks(query.UserId)).ReturnsAsync(linkStatistics);
            var result = await _handler.Handle(query, CancellationToken.None);
            Assert.NotNull(result);
            Assert.Equal(1, result.userId);
            Assert.Equal(2, result.monthlyClicksByDay.Count);
        }

        [Fact]
        public async Task Handle_ThrowsKeyNotFoundException_WhenUserDoesNotExist()
        {
            var query = new GetMonthlyClicksByDayQuery(userId);
            _mockValidationService.Setup(v => v.IsValidId(query.UserId));
            _mockUserRepository.Setup(repo => repo.GetById(query.UserId)).ReturnsAsync((User)null);
            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _handler.Handle(query, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ThrowsApplicationException_WhenExceptionOccurs()
        {
            var query = new GetMonthlyClicksByDayQuery(userId);
            _mockValidationService.Setup(v => v.IsValidId(query.UserId));
            _mockUserRepository.Setup(repo => repo.GetById(query.UserId)).ThrowsAsync(new Exception("Database error"));
            await Assert.ThrowsAsync<ApplicationException>(() =>
                _handler.Handle(query, CancellationToken.None));
        }
    }
}
