
using AutoMapper;
using Moq;
using ShortenedLinks.Application.DTO.Link;
using ShortenedLinks.Application.Features.Links.Queries.GetById;
using ShortenedLinks.Application.Interfaces;
using ShortenedLinks.Application.Services.Validation;
using ShortenedLinks.Domain.Entities;
using ShortenedLinks.Domain.Interfaces.Repositories;

namespace ShortenedLinks.Tests.ApplicationTest.Features.LinkTest.Queries
{
    public class GetByIdLinkQueryTests
    {
        private readonly Mock<ILinkRepository> _mockLinkRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IValidationService> _mockValidationService;
        private readonly GetByIdLinkQueryHandler _handler;
        Link link = new Link { Id = 1, OriginalLink = "http://web.com", ShortenedLink = "OOFAZ1" };
        LinkDetailsDTO linkDetailsDTO = new LinkDetailsDTO { Id = 1, OriginalLink = "http://web.com", ShortenedLink = "OOFAZ1" };
        int linkId = 1;
        public GetByIdLinkQueryTests()
        {
            _mockLinkRepository = new Mock<ILinkRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockValidationService = new Mock<IValidationService>();
            _handler = new GetByIdLinkQueryHandler(_mockLinkRepository.Object, _mockMapper.Object, _mockValidationService.Object);
        }

        [Fact]
        public async Task Handle_ReturnsLinkDetailsDTO_WhenLinkExists()
        {
            _mockValidationService.Setup(service => service.IsValidId(It.IsAny<int>())).Verifiable();
            _mockLinkRepository.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync(link);
            _mockMapper.Setup(m => m.Map<LinkDetailsDTO>(link)).Returns(linkDetailsDTO);
            var query = new GetByIdLinkQuery(linkId);
            var result = await _handler.Handle(query, CancellationToken.None);
            Assert.NotNull(result);
            Assert.Equal(linkDetailsDTO.Id, result.Id);
            Assert.Equal(linkDetailsDTO.OriginalLink, result.OriginalLink);
            Assert.Equal(linkDetailsDTO.ShortenedLink, result.ShortenedLink);
        }
        [Fact]
        public async Task Handle_ThrowsKeyNotFoundException_WhenLinkDoesNotExist()
        {
            int linkId = 2;
            _mockValidationService.Setup(service => service.IsValidId(It.IsAny<int>())).Verifiable();
            _mockLinkRepository.Setup(repo => repo.GetById(It.IsAny<int>())).ReturnsAsync((Link)null);
            var query = new GetByIdLinkQuery(linkId);
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(query, CancellationToken.None));
        }
        [Fact]
        public async Task Handle_ThrowsArgumentException_WhenIdIsInvalid()
        {
            int linkId = 0;
            _mockValidationService.Setup(service => service.IsValidId(It.IsAny<int>())).Throws<ArgumentException>();
            var query = new GetByIdLinkQuery(linkId);
            await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(query, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ThrowsApplicationException_WhenAnErrorOccurs()
        {
            _mockValidationService.Setup(service => service.IsValidId(It.IsAny<int>())).Verifiable();
            _mockLinkRepository.Setup(repo => repo.GetById(It.IsAny<int>())).ThrowsAsync(new Exception("Test exception"));
            var query = new GetByIdLinkQuery(linkId);
            await Assert.ThrowsAsync<ApplicationException>(() => _handler.Handle(query, CancellationToken.None));
        }
    }
}
