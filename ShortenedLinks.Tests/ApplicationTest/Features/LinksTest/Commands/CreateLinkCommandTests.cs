using AutoMapper;
using Moq;
using ShortenedLinks.Application.DTO.Link;
using ShortenedLinks.Application.Features.Links.Commands.Create;
using ShortenedLinks.Application.Interfaces;
using ShortenedLinks.Application.Services.Links;
using ShortenedLinks.Domain.Entities;
using ShortenedLinks.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortenedLinks.Tests.ApplicationTest.Features.LinkTest.Commands
{
    public class CreateLinkCommandTests
    {
        private readonly Mock<ILinkRepository> _mockLinkRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IValidationService> _mockValidationService;
        private readonly Mock<ILinkShortenedService> _mockLinkShortenerService;
        private readonly CreateLinkCommandHandler _handler;
        string invalidLink = "invalid-link";
        string validLink = "http://web.com";
        string shortLink = "OOFAZ1";
        Link newLink = new Link { OriginalLink = "http://web.com", ShortenedLink = "OOFAZ1", CreatedAt = DateTime.UtcNow, UserId = 1, Id = 1 };
        LinkListDTO linkListDTO = new LinkListDTO { OriginalLink = "http://web.com", ShortenedLink = "OOFAZ1", Id = 1 };
        public CreateLinkCommandTests()
        {
            _mockLinkRepository = new Mock<ILinkRepository>();
            _mockMapper = new Mock<IMapper>();
            _mockValidationService = new Mock<IValidationService>();
            _mockLinkShortenerService = new Mock<ILinkShortenedService>();
            _handler = new CreateLinkCommandHandler(
                _mockLinkRepository.Object,
                _mockLinkShortenerService.Object,
                _mockValidationService.Object,
                _mockMapper.Object);
        }
        [Fact]
        public async Task Handle_ShouldThrowArgumentException_WhenLinkIsInvalid()
        {
            var command = new CreateLinkCommand(new LinkCreateDTO { Link = invalidLink, UserId = 1 });
            _mockValidationService.Setup(service => service.IsValidLink(invalidLink))
                .Throws(new ArgumentException("The provided Link is not valid."));
            await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrowInvalidOperationException_WhenLinkCreationFails()
        {
            var command = new CreateLinkCommand(new LinkCreateDTO { Link = validLink, UserId = 1 });
            _mockValidationService.Setup(service => service.IsValidLink(validLink)).Verifiable();
            _mockLinkShortenerService.Setup(service => service.GenerateShortLink())
                .ReturnsAsync(shortLink);
            _mockLinkRepository.Setup(repo => repo.Create(It.IsAny<Link>()))
                .ReturnsAsync((Link)null);
            await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldReturnLinkListDTO_WhenLinkIsCreatedSuccessfully()
        {
            var command = new CreateLinkCommand(new LinkCreateDTO { Link = validLink, UserId = 1 });
            _mockValidationService.Setup(service => service.IsValidLink(validLink)).Verifiable();
            _mockLinkShortenerService.Setup(service => service.GenerateShortLink())
                .ReturnsAsync(shortLink);
            _mockLinkRepository.Setup(repo => repo.Create(It.IsAny<Link>()))
                .ReturnsAsync(newLink);
            _mockMapper.Setup(mapper => mapper.Map<LinkListDTO>(newLink))
                .Returns(linkListDTO);
            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(validLink, result.OriginalLink);
            Assert.Equal(shortLink, result.ShortenedLink);
            Assert.Equal(1, result.Id);
        }
    }
}
