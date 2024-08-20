﻿using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShortenedLinks.Application.DTO.LinkStatistic;
using ShortenedLinks.Application.DTO.LinkStatistic.MonthlyClicksByDayDTO;
using ShortenedLinks.Application.DTO.Response;
using ShortenedLinks.Application.Features.LinksStatistics.Queries.GetMonthlyClicksByDay;
using ShortenedLinks.Application.Features.LinksStatistics.Queries.GetTopLinks;
using ShortenedLinks.Domain.Enums;


namespace ShortenedLinks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LinkStats : ControllerBase
    {
        private readonly IMediator _mediator;

        public LinkStats(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("top-link")]
        public async Task<ActionResult<Response<List<LinkClicksStatisticTopDTO>>>> GetTopLinks([FromQuery] int userId, [FromQuery] PeriodType period = PeriodType.Month, [FromQuery] int topN = 10)
        {
            var rsp = new Response<List<LinkClicksStatisticTopDTO>>();
            try
            {
                var query = new GetTopLinksQuery(userId, period, topN);
                rsp.status = true;
                rsp.value = await _mediator.Send(query);
                rsp.msg = $"Top {topN} most clicked links of {period}";
            }
            catch (Exception ex)
            {                rsp.status = false;
                rsp.status = false;
                rsp.msg = $"An error occurred: {ex.Message}";
                return StatusCode(500, rsp);
            }
            return Ok(rsp);
        }
        [HttpGet("monthly-clicks/{userId:int}")]
        public async Task<ActionResult<Response<MonthlyClicksByDayDTO>>> GetMonthlyClicks(int userId)
        {
            var rsp = new Response<MonthlyClicksByDayDTO>();
            try
            {
                var query = new GetMonthlyClicksByDayQuery(userId);
                rsp.status = true;
                rsp.value = await _mediator.Send(query);
                rsp.msg = $"Monthly Clicks by Day";
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
