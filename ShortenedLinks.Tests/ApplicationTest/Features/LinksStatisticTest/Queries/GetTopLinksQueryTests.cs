using AutoMapper;
using Moq;
using ShortenedLinks.Application.DTO.Link;
using ShortenedLinks.Application.Features.LinksStatistics.Queries.GetTopLinks;
using ShortenedLinks.Application.Interfaces;
using ShortenedLinks.Domain.Entities;
using ShortenedLinks.Domain.Enums;
using ShortenedLinks.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortenedLinks.Tests.ApplicationTest.Features.LinksStatisticTest.Queries
{
    public class GetTopLinksQueryTests
    {
        private readonly Mock<ILinkStatisticRepository> _mockLinkStatisticRepository;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IValidationService> _mockValidationService;
        private readonly GetTopLinksQueryHandler _handler;
        int userId = 1, TopN = 5;
        User user = new User { Id = 1 };
        new List<LinkStatistic> linkStatistics = new List<LinkStatistic>
        {
            new LinkStatistic { LinkId = 1, Link = new Link { Id = 1, ShortenedLink = "short1" } },
            new LinkStatistic { LinkId = 1, Link = new Link { Id = 1, ShortenedLink = "short1" } },
            new LinkStatistic { LinkId = 2, Link = new Link { Id = 2, ShortenedLink = "short2" } }
        };
        public GetTopLinksQueryTests()
        {
            _mockLinkStatisticRepository = new Mock<ILinkStatisticRepository>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockValidationService = new Mock<IValidationService>();
            _handler = new GetTopLinksQueryHandler(
                _mockLinkStatisticRepository.Object,
                _mockUserRepository.Object,
                _mockMapper.Object,
                _mockValidationService.Object
            );
        }
        [Fact]
        public async Task Handle_ReturnsTopLinks_WhenUserExists()
        {
            var query = new GetTopLinksQuery(userId, PeriodType.Month, TopN);
            _mockValidationService.Setup(v => v.IsValidId(query.UserId));
            _mockUserRepository.Setup(repo => repo.GetById(query.UserId)).ReturnsAsync(user);
            _mockLinkStatisticRepository.Setup(repo => repo.GetLinksByRangePeriod(query.UserId, query.PeriodType))
                .ReturnsAsync(linkStatistics);
            _mockMapper.Setup(m => m.Map<LinkListDTO>(It.IsAny<Link>()))
                .Returns((Link source) => new LinkListDTO { Id = source.Id, ShortenedLink = source.ShortenedLink });

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal(2, result[0].ClickCount);
            Assert.Equal(1, result[1].ClickCount);
        }

        [Fact]
        public async Task Handle_ThrowsKeyNotFoundException_WhenUserDoesNotExist()
        {
            var query = new GetTopLinksQuery(userId, PeriodType.Month, TopN);
            _mockValidationService.Setup(v => v.IsValidId(query.UserId));
            _mockUserRepository.Setup(repo => repo.GetById(query.UserId)).ReturnsAsync((User)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _handler.Handle(query, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ThrowsApplicationException_WhenExceptionOccurs()
        {
            var query = new GetTopLinksQuery(userId, PeriodType.Month, TopN);
            _mockValidationService.Setup(v => v.IsValidId(query.UserId));
            _mockUserRepository.Setup(repo => repo.GetById(query.UserId)).ReturnsAsync(user);
            _mockLinkStatisticRepository.Setup(repo => repo.GetLinksByRangePeriod(query.UserId, query.PeriodType))
                .ThrowsAsync(new Exception("Simulated exception"));

            await Assert.ThrowsAsync<ApplicationException>(() =>
                _handler.Handle(query, CancellationToken.None));
        }
    }
}
