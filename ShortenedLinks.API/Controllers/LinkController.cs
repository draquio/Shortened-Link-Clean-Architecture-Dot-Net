using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShortenedLinks.Application.DTO.Link;
using ShortenedLinks.Application.DTO.Response;
using ShortenedLinks.Application.Features.Links.Commands.Create;
using ShortenedLinks.Application.Features.Links.Commands.Delete;
using ShortenedLinks.Application.Features.Links.Queries.GetAll;
using ShortenedLinks.Application.Features.Links.Queries.GetById;
using System.Collections.Generic;

namespace ShortenedLinks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LinkController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LinkController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<ActionResult<ResponseList<List<LinkDetailsDTO>>>> GetAll(int page = 1, int pageSize = 10)
        {
            var rsp = new ResponseList<List<LinkDetailsDTO>>();
            try
            {
                var query = new GetAllLinksQuery(page, pageSize);
                rsp.value = await _mediator.Send(query);
                rsp.status = true;
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = $"An error occurred: {ex.Message}";
                return StatusCode(500, rsp);
            }
            return Ok(rsp);
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Response<LinkDetailsDTO>>> GetById(int id)
        {
            var rsp = new Response<LinkDetailsDTO>();
            try
            {
                var query = new GetByIdLinkQuery(id);
                rsp.status = true;
                rsp.value = await _mediator.Send(query);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = $"An error occurred: {ex.Message}";
                return StatusCode(500, rsp);
            }
            return Ok(rsp);
        }

        [HttpPost]
        public async Task<ActionResult<Response<LinkListDTO>>> Create([FromBody] LinkCreateDTO link)
        {
            var rsp = new Response<LinkListDTO>();
            try
            {
                var command = new CreateLinkCommand(link);
                rsp.status = true;
                rsp.value = await _mediator.Send(command);
                rsp.msg = "Short link created successfully";
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = $"An error occurred: {ex.Message}";
                return StatusCode(500, rsp);
            }
            return Ok(rsp);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Response<bool>>> Delete(int id)
        {
            var rsp = new Response<bool>();
            try
            {
                var command = new DeleteLinkCommand(id);
                rsp.status = true;
                rsp.value = await _mediator.Send(command);
                rsp.msg = "Short link deleted successfully";
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
