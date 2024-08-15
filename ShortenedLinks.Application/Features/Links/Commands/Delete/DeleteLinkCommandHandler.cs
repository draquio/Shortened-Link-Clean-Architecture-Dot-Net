using MediatR;
using ShortenedLinks.Application.Services.Validation;
using ShortenedLinks.Domain.Entities;
using ShortenedLinks.Domain.Interfaces.Repositories;

namespace ShortenedLinks.Application.Features.Links.Commands.Delete
{
    public class DeleteLinkCommandHandler : IRequestHandler<DeleteLinkCommand, bool>
    {
        private readonly ILinkRepository _linkRepository;
        private readonly ValidationService _validationService;

        public DeleteLinkCommandHandler(ILinkRepository linkRepository, ValidationService validationService)
        {
            _linkRepository = linkRepository;
            _validationService = validationService;
        }

        public async Task<bool> Handle(DeleteLinkCommand command, CancellationToken cancellationToken)
        {
            try
            {
                _validationService.IsValidId(command.Id);
                Link link = await _linkRepository.GetById(command.Id);
                if (link == null) throw new KeyNotFoundException($"Link with ID {command.Id} not found");
                bool response = await _linkRepository.Delete(link);
                if (!response) throw new InvalidOperationException("Link could not be deleted");
                return response;
            }
            catch (InvalidOperationException) { throw; }
            catch (ArgumentException) { throw; }
            catch (KeyNotFoundException) { throw; }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while deleting the short link.", ex);
            }
        }
    }
}
