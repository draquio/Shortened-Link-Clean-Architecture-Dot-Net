using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShortenedLinks.Application.DTO.LinkStatistic;
using ShortenedLinks.Application.DTO.Response;
using ShortenedLinks.Application.DTO.ShortLink;
using ShortenedLinks.Application.Features.LinksStatistics.Commands.Create;
using ShortenedLinks.Application.Features.ShortLink.Queries.GetByShortLink;

namespace ShortenedLinks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShortLinkController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ShortLinkController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{code}")]
        public async Task<ActionResult<Response<ShortLinkDetailDTO>>> GetByShortCode(string code)
        {
            var rsp = new Response<ShortLinkDetailDTO>();
            try
            {
                // Get for Code
                var query = new GetByShortLinkQuery(code);
                var result = await _mediator.Send(query);

                // Post for visit
                var visitorIp = HttpContext.Connection.RemoteIpAddress?.ToString();
                int linkId = result.Id;
                var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();
                var command = new RegisterLinkStatisticCommand(linkId, visitorIp, userAgent);
                await _mediator.Send(command);

                rsp.value = result;
                rsp.status = true;
                rsp.value = result;
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = $"An error occurred: {ex.Message}";
                return StatusCode(500, rsp);
            }
            return Ok(rsp);
        }

    }
}
