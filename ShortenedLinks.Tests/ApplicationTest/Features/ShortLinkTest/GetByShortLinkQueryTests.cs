using AutoMapper;
using Moq;
using ShortenedLinks.Application.DTO.ShortLink;
using ShortenedLinks.Application.Features.ShortLink.Queries.GetByShortLink;
using ShortenedLinks.Domain.Entities;
using ShortenedLinks.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortenedLinks.Tests.ApplicationTest.Features.ShortLinkTest
{
    public class GetByShortLinkQueryTests
    {
        private readonly Mock<ILinkRepository> _mockLinkRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly GetByShortLinkQueryHandler _handler;
        string shortCode = "OOFAZ1";
        Link link = new Link { ShortenedLink = "OOFAZ1", OriginalLink = "http://web.com" };
        ShortLinkDetailDTO shortLinkDetailDTO = new ShortLinkDetailDTO { ShortenedLink = "OOFAZ1", OriginalLink = "http://web.com" };
        public GetByShortLinkQueryTests()
        {
            _mockLinkRepository = new Mock<ILinkRepository>();
            _mockMapper = new Mock<IMapper>();
            _handler = new GetByShortLinkQueryHandler(_mockLinkRepository.Object, _mockMapper.Object);
        }
        [Fact]
        public async Task Handle_ShouldReturnShortLinkDetailDTO_WhenLinkExists()
        {
            _mockLinkRepository.Setup(repo => repo.GetByShortCode(shortCode))
                .ReturnsAsync(link);
            _mockMapper.Setup(mapper => mapper.Map<ShortLinkDetailDTO>(link))
                .Returns(shortLinkDetailDTO);
            var result = await _handler.Handle(new GetByShortLinkQuery(shortCode), CancellationToken.None);
            Assert.NotNull(result);
            Assert.Equal(shortLinkDetailDTO.ShortenedLink, result.ShortenedLink);
            Assert.Equal(shortLinkDetailDTO.OriginalLink, result.OriginalLink);
        }

        [Fact]
        public async Task Handle_ShouldThrowKeyNotFoundException_WhenLinkDoesNotExist()
        {
            _mockLinkRepository.Setup(repo => repo.GetByShortCode(shortCode))
                .ReturnsAsync((Link)null);
            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _handler.Handle(new GetByShortLinkQuery(shortCode), CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrowApplicationException_WhenExceptionOccurs()
        {
            _mockLinkRepository.Setup(repo => repo.GetByShortCode(shortCode))
                .ThrowsAsync(new Exception("Database error"));
            await Assert.ThrowsAsync<ApplicationException>(() =>
                _handler.Handle(new GetByShortLinkQuery(shortCode), CancellationToken.None));
        }
    }
}
