using AutoMapper;
using ShortenedLinks.Application.Features.Links.Queries.GetAll;
using ShortenedLinks.Domain.Interfaces.Repositories;
using Moq;
using ShortenedLinks.Application.DTO.Link;
using ShortenedLinks.Domain.Entities;

namespace ShortenedLinks.Tests.ApplicationTest.Features.LinkTest.Queries
{
    public class GetAllLinksQueryTests
    {
        private readonly Mock<ILinkRepository> _mockLinkRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly GetAllLinksQueryHandler _handler;
        List<Link> links = new List<Link>
        {
            new Link { Id = 1, OriginalLink = "http://web.com", ShortenedLink = "OOFAZ1" },
            new Link { Id = 2, OriginalLink = "http://web2.com", ShortenedLink = "q0YTIR" }
        };
        List<LinkDetailsDTO> linkDTOs = new List<LinkDetailsDTO>
        {
            new LinkDetailsDTO { Id = 1, OriginalLink = "http://web.com", ShortenedLink = "OOFAZ1" },
            new LinkDetailsDTO { Id = 2, OriginalLink = "http://web2.com", ShortenedLink = "q0YTIR" }
        };
        int page = 1, pageSize = 10;
        public GetAllLinksQueryTests()
        {
            _mockLinkRepository = new Mock<ILinkRepository>();
            _mockMapper = new Mock<IMapper>();
            _handler = new GetAllLinksQueryHandler(_mockLinkRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Handle_ReturnsLinkDetailsDTOList_WhenLinksExist()
        {
            _mockLinkRepository.Setup(repo => repo.GetAllWithUsername(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(links);
            _mockMapper.Setup(m => m.Map<List<LinkDetailsDTO>>(links)).Returns(linkDTOs);
            var query = new GetAllLinksQuery(page, pageSize);
            var result = await _handler.Handle(query, CancellationToken.None);
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal(linkDTOs, result);
        }
        [Fact]
        public async Task Handle_ReturnsEmptyList_WhenLinksDoNotExist()
        {
            _mockLinkRepository.Setup(repo => repo.GetAllWithUsername(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync((List<Link>)null);
            var query = new GetAllLinksQuery(page, pageSize);
            var result = await _handler.Handle(query, CancellationToken.None);
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task Handle_ThrowsApplicationException_WhenErrorOccurs()
        {
            _mockLinkRepository.Setup(repo => repo.GetAllWithUsername(It.IsAny<int>(), It.IsAny<int>()))
                .ThrowsAsync(new Exception("Database error"));
            var query = new GetAllLinksQuery(page, pageSize);
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => _handler.Handle(query, CancellationToken.None));
            Assert.Equal("An error occurred while getting the short links.", exception.Message);
        }
    }
}
