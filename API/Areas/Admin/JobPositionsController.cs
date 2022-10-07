﻿using Application.Common.DTO.JobPositions;
using Application.JobPositions.Commands.CreateJobPosition;
using Application.JobPositions.Commands.DeleteJobPosition;
using Application.JobPositions.Commands.UpdateJobPosition;
using Application.JobPositions.Queries.GetJobPositions;
using Application.Tags.Queries.GetJobPosition;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using API.Authorize;

namespace API.Areas.Admin;

[Authorize("Admin", "SuperAdmin")]
[Route("api/Admin/[controller]")]
public class JobPositionsController : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<JobPositionDTO>))]
    public async Task<IActionResult> GetJobPositions(
        [FromQuery] string? orderByExpression,
        [FromQuery] string? searchTerm,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await Mediator.Send(new GetJobPositionsWithPaginationWithSearchWithSortQuery
        {
            PageSize = pageSize,
            PageNumber = pageNumber,

            SearchTerm = searchTerm ?? string.Empty,

            OrderByExpression = orderByExpression ?? "Name"
        });

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

        return Ok(result);
    }

    [HttpGet("{jobPositionId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JobPositionDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetJobPosition(Guid jobPositionId)
    {
        return Ok(await Mediator.Send(new GetJobPositionQuery(jobPositionId)));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
    public async Task<IActionResult> CreateJobPosition(CreateJobPositionCommand command)
    {
        var result = await Mediator.Send(command);

        return Ok(result);
    }

    [HttpPut("{jobPositionId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateJobPosition(Guid jobPositionId, UpdateJobPositionCommand command)
    {
        if (jobPositionId != command.JobPositionId)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpDelete("{jobPositionId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteJobPosition(Guid jobPositionId)
    {
        await Mediator.Send(new DeleteJobpositionCommand(jobPositionId));

        return NoContent();
    }
}