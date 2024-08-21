using Moq;
using ShortenedLinks.Application.Features.Links.Commands.Delete;
using ShortenedLinks.Application.Interfaces;
using ShortenedLinks.Domain.Entities;
using ShortenedLinks.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortenedLinks.Tests.ApplicationTest.Features.LinkTest.Commands
{
    public class DeleteLinkCommandTests
    {
        private readonly Mock<ILinkRepository> _mockLinkRepository;
        private readonly Mock<IValidationService> _mockValidationService;
        private readonly DeleteLinkCommandHandler _handler;
        int invalidId = -1, validId = 1;
        Link link = new Link { Id = 1 };
        public DeleteLinkCommandTests()
        {
            _mockLinkRepository = new Mock<ILinkRepository>();
            _mockValidationService = new Mock<IValidationService>();
            _handler = new DeleteLinkCommandHandler(_mockLinkRepository.Object, _mockValidationService.Object);
        }

        [Fact]
        public async Task Handle_ShouldThrowArgumentException_WhenIdIsInvalid()
        {
            var command = new DeleteLinkCommand(invalidId);
            _mockValidationService.Setup(service => service.IsValidId(invalidId))
                .Throws(new ArgumentException("Invalid ID."));
            await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, CancellationToken.None));
        }
        [Fact]
        public async Task Handle_ShouldThrowKeyNotFoundException_WhenLinkNotFound()
        {
            var command = new DeleteLinkCommand(validId);
            _mockValidationService.Setup(service => service.IsValidId(validId));
            _mockLinkRepository.Setup(repo => repo.GetById(validId))
                .ReturnsAsync((Link)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrowInvalidOperationException_WhenDeleteFails()
        {
            var command = new DeleteLinkCommand(validId);
            _mockValidationService.Setup(service => service.IsValidId(validId));
            _mockLinkRepository.Setup(repo => repo.GetById(validId))
                .ReturnsAsync(link);
            _mockLinkRepository.Setup(repo => repo.Delete(link))
                .ReturnsAsync(false);
            await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
        }
        [Fact]
        public async Task Handle_ShouldReturnTrue_WhenDeleteIsSuccessful()
        {
            var command = new DeleteLinkCommand(validId);
            _mockValidationService.Setup(service => service.IsValidId(validId));
            _mockLinkRepository.Setup(repo => repo.GetById(validId))
                .ReturnsAsync(link);
            _mockLinkRepository.Setup(repo => repo.Delete(link))
                .ReturnsAsync(true);
            var result = await _handler.Handle(command, CancellationToken.None);
            Assert.True(result);
        }
    }
}
