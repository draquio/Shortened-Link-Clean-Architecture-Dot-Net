
using AutoMapper;
using MediatR;
using ShortenedLinks.Application.DTO.Link;
using ShortenedLinks.Application.Interfaces;

using ShortenedLinks.Domain.Entities;
using ShortenedLinks.Domain.Interfaces.Repositories;

namespace ShortenedLinks.Application.Features.Links.Commands.Create
{
    public class CreateLinkCommandHandler : IRequestHandler<CreateLinkCommand, LinkListDTO>
    {
        private readonly ILinkRepository _linkRepository;
        private readonly IMapper _mapper;
        private readonly IValidationService _validationService;
        private readonly ILinkShortenedService _linkShortenerService;

        public CreateLinkCommandHandler(ILinkRepository linkRepository,
            ILinkShortenedService linkShortenerService,
            IValidationService validationService, 
            IMapper mapper)
        {
            _linkRepository = linkRepository;
            _linkShortenerService = linkShortenerService;
            _validationService = validationService;
            _mapper = mapper;
        }

        public async Task<LinkListDTO> Handle(CreateLinkCommand request, CancellationToken cancellationToken)
        {
            try
            {
                string link = request.Link.Link;
                int userId = request.Link.UserId;
                _validationService.IsValidLink(link);
                string shorLink = await _linkShortenerService.GenerateShortLink();
                Link newShortenedLink = new Link
                {
                    OriginalLink = link,
                    ShortenedLink = shorLink,
                    CreatedAt = DateTime.UtcNow,
                    UserId = userId
                };
                Link createdShortenedLink = await _linkRepository.Create(newShortenedLink);
                if (createdShortenedLink == null || createdShortenedLink.Id == 0) throw new InvalidOperationException("Link could not be created");
                LinkListDTO linkListDTO = _mapper.Map<LinkListDTO>(createdShortenedLink);
                return linkListDTO;
            }
            catch (ArgumentException) { throw; }
            catch (InvalidOperationException) { throw; }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while creating the short link.", ex);
            }
        }
    }
}
