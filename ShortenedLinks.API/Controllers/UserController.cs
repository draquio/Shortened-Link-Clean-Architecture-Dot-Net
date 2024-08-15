using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShortenedLinks.Application.DTO.Response;
using ShortenedLinks.Application.DTO.User;
using ShortenedLinks.Application.Features.Users.Commands.Create;
using ShortenedLinks.Application.Features.Users.Commands.Delete;
using ShortenedLinks.Application.Features.Users.Commands.Update;
using ShortenedLinks.Application.Features.Users.Queries.GetAll;
using ShortenedLinks.Application.Features.Users.Queries.GetById;

namespace ShortenedLinks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseList<List<UserReadDTO>>>> GetAll(int page = 1, int pageSize = 10)
        {
            var rsp = new ResponseList<List<UserReadDTO>>();
            try
            {
                var query = new GetAllUsersQuery(page, pageSize);
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
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Response<UserReadDTO>>> GetByID(int id)
        {
            var rsp = new Response<UserReadDTO>();
            try
            {
                var query = new GetByIdUserQuery(id);
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
        public async Task<ActionResult<Response<UserReadDTO>>> Create([FromBody] UserCreateDTO userCreateDTO)
        {
            var rsp = new Response<UserReadDTO>();
            try
            {
                if(userCreateDTO == null)
                {
                    rsp.status= false;
                    rsp.msg = "User can not be null";
                    return BadRequest(rsp);
                }
                var command = new CreateUserCommand(userCreateDTO);
                rsp.status = true;
                rsp.msg = "User created successfully";
                rsp.value = await _mediator.Send(command);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = $"An error occurred: {ex.Message}";
                return StatusCode(500, rsp);
            }
            return Ok(rsp);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Response<bool>>> Update([FromBody] UserUpdateDTO userUpdateDTO, int id)
        {
            var rsp = new Response<bool>();
            try
            {
                if (userUpdateDTO == null || userUpdateDTO.Id != id)
                {
                    rsp.status = false;
                    rsp.msg = "User can not be null or ID mismatch";
                    return BadRequest(rsp);
                }
                var command = new UpdateUserCommand(userUpdateDTO);
                rsp.status = true;
                rsp.value = await _mediator.Send(command);
                rsp.msg = "User updated successfully";
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
                if (id <= 0)
                {
                    rsp.status = false;
                    rsp.msg = "Invalid ID";
                    return BadRequest(rsp);
                }
                var command = new DeleteUserCommand(id);
                rsp.status = true;
                rsp.value = await _mediator.Send(command);
                rsp.msg = "User deleted successfully";
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
