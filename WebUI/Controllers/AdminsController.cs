using Application.Admins.Queries;
using Application.Common.Models.Filtration.Admin;
using Application.Common.Models.Pagination;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebUI.Authorize;
using WebUI.DTO.Admin;

namespace WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("Admin")]
    public class AdminsController : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdminResponse>>> GetAdmins([FromQuery] PaginationParameters paginationParameters, [FromQuery] AdminListFilter filter)
        {
            var result = await Mediator.Send(new GetAdminsQuery
            {
                PaginationParameters = paginationParameters,
                FilterParameters = new AdminListFilterParameters { WithoutAdminId = AccountInfo!.Id, IsVerified = filter.IsVerified }
            });

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));

            return Ok(result.Select(x => new AdminResponse(x)));
        }

        [HttpGet("{adminId}")]
        public async Task<ActionResult<AdminResponse>> GetAdmin(Guid adminId)
        {
            var result = await Mediator.Send(new GetAdminQuery
            {
                AdminId = adminId
            });

            return new AdminResponse(result);
        }
    }
}
