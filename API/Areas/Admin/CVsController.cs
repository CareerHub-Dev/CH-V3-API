using API.Authorize;
using API.DTO.Responses;
using Application.Common.Enums;
using Application.CVs.Commands.CreateCV;
using Application.CVs.Commands.DeleteCV;
using Application.CVs.Commands.UpdateCVDetail;
using Application.CVs.Commands.UpdateCVPhoto;
using Microsoft.AspNetCore.Mvc;

namespace API.Areas.Admin;

[Authorize(Role.Admin, Role.SuperAdmin)]
[Route("api/Admin/[controller]")]
public class CVsController : ApiControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
    public async Task<IActionResult> CreateCV(CreateCVCommand command)
    {
        var result = await Sender.Send(command);

        return Ok(result);
    }

    [HttpPut("{cvId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateCVDetail(Guid cvId, UpdateCVDetailCommand command)
    {
        if (cvId != command.CVId)
        {
            return BadRequest();
        }

        await Sender.Send(command);

        return NoContent();
    }

    [HttpPost("{cvId}/photo")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ImageResponse))]
    public async Task<IActionResult> UpdateCVPhoto(Guid cvId, IFormFile? file)
    {
        var result = await Sender.Send(new UpdateCVPhotoCommand
        {
            CVId = cvId,
            Photo = file
        });

        return Ok(new ImageResponse { Route = result });
    }

    [HttpDelete("{cvId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCV(Guid cvId)
    {
        await Sender.Send(new DeleteCVCommand(cvId));

        return NoContent();
    }
}
