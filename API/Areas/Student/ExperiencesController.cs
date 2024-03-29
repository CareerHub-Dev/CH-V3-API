﻿using API.Authorize;
using API.DTO.Requests.Experiences;
using Application.Common.DTO.Experiences;
using Application.Common.Enums;
using Application.Experiences.Commands.CreateExperience;
using Application.Experiences.Commands.DeleteExperienceOfStudent;
using Application.Experiences.Commands.UpdateExperienceOfStudent;
using Application.Experiences.Queries.GetExperienceOfStudent;
using Application.Experiences.Queries.GetExperiencesOfStudentWithPaging;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace API.Areas.Student;

[Authorize(Role.Student)]
[Route("api/Student/self/[controller]")]
public class ExperiencesController : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ExperienceDTO>))]
    public async Task<IActionResult> GetExperiencesOfSelfStudent(
        [FromQuery] string? order,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await Sender.Send(new GetExperiencesOfStudentWithPagingQuery
        {
            StudentId = AccountInfo!.Id,

            PageNumber = pageNumber,
            PageSize = pageSize,

            OrderByExpression = order ?? "Title"
        });

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

        return Ok(result);
    }

    [HttpGet("{experienceId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ExperienceDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetExperienceOfSelfStudent(Guid experienceId)
    {
        return Ok(await Sender.Send(new GetExperienceOfStudentQuery
        {
            ExperienceId = experienceId,
            StudentId = AccountInfo!.Id
        }));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
    public async Task<IActionResult> CreateExperienceForSelfStudent(CreateOwnExperienceRequest request)
    {
        var result = await Sender.Send(new CreateExperienceCommand
        {
            Title = request.Title,
            CompanyName = request.CompanyName,
            JobType = request.JobType,
            WorkFormat = request.WorkFormat,
            ExperienceLevel = request.ExperienceLevel,
            JobLocation = request.JobLocation,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            StudentId = AccountInfo!.Id
        });

        return Ok(result);
    }

    [HttpDelete("{experienceId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteExperienceOfSelfStudent(Guid experienceId)
    {
        await Sender.Send(new DeleteExperienceOfStudentCommand(experienceId, AccountInfo!.Id));

        return NoContent();
    }

    [HttpPut("{experienceId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateExperienceOfSelfStudent(Guid experienceId, UpdateOwnExperienceRequest request)
    {
        if (experienceId != request.ExperienceId)
        {
            return BadRequest();
        }

        await Sender.Send(new UpdateExperienceOfStudentCommand
        {
            ExperienceId = request.ExperienceId,
            Title = request.Title,
            CompanyName = request.CompanyName,
            JobType = request.JobType,
            WorkFormat = request.WorkFormat,
            ExperienceLevel = request.ExperienceLevel,
            JobLocation = request.JobLocation,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            StudentId = AccountInfo!.Id
        });

        return NoContent();
    }
}
