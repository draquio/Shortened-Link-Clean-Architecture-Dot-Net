using AutoMapper;
using MediatR;
using ShortenedLinks.Application.DTO.Link;
using ShortenedLinks.Application.Interfaces;
using ShortenedLinks.Application.Services.Validation;
using ShortenedLinks.Domain.Entities;
using ShortenedLinks.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortenedLinks.Application.Features.Links.Queries.GetById
{
    public class GetByIdLinkQueryHandler : IRequestHandler<GetByIdLinkQuery, LinkDetailsDTO>
    {
        private readonly ILinkRepository _linkRepository;
        private readonly IMapper _mapper;
        private readonly IValidationService _validationService;

        public GetByIdLinkQueryHandler(ILinkRepository linkRepository, IMapper mapper, IValidationService validationService)
        {
            _linkRepository = linkRepository;
            _mapper = mapper;
            _validationService = validationService;
        }

        public async Task<LinkDetailsDTO> Handle(GetByIdLinkQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _validationService.IsValidId(request.Id);
                Link link = await _linkRepository.GetById(request.Id);
                if (link == null) throw new KeyNotFoundException($"Link with ID {request.Id} not found");
                LinkDetailsDTO linkDetailsDTO = _mapper.Map<LinkDetailsDTO>(link);
                return linkDetailsDTO;
            }
            catch (ArgumentException) { throw; }
            catch (KeyNotFoundException) { throw; }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while getting the short link.", ex);
            }
        }
    }
}
