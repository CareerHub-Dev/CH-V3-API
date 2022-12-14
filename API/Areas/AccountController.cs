using API.DTO.Requests.RefreshTokens;
using Application.Accounts.Commands.RegisterStudent;
using Application.Accounts.Commands.ResetPassword;
using Application.Accounts.Commands.VerifyAdminWithContinuedRegistration;
using Application.Accounts.Commands.VerifyCompanyWithContinuedRegistration;
using Application.Accounts.Commands.VerifyStudent;
using Application.Accounts.Queries.Authenticate;
using Application.Accounts.Queries.RefreshToken;
using Application.Emails.Commands.SendPasswordResetEmail;
using Microsoft.AspNetCore.Mvc;

namespace API.Areas;

[Route("api/[controller]")]
public class AccountController : ApiControllerBase
{
    [HttpPost("authenticate")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthenticateResult))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Authenticate(AuthenticateQuery query)
    {
        var response = await Sender.Send(query);

        SetTokenCookie(response.RefreshToken);
        return Ok(response);
    }

    [HttpPost("refresh-token")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RefreshTokenResult))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RefreshToken(RefreshTokenRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Token))
        {
            request.Token = Request.Cookies["refreshToken"] ?? "";
        }

        var response = await Sender.Send(new RefreshTokenQuery
        {
            Token = request.Token
        });

        SetTokenCookie(response.RefreshToken);
        return Ok(response);
    }

    [HttpPost("verify-company-email")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> VerifyCompanyWithContinuedRegistration([FromForm] VerifyCompanyWithContinuedRegistrationCommand command)
    {
        await Sender.Send(command);

        return NoContent();
    }

    [HttpPost("verify-admin-email")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> VerifyAdminWithContinuedRegistration(VerifyAdminWithContinuedRegistrationCommand command)
    {
        await Sender.Send(command);

        return NoContent();
    }

    [HttpPost("verify-student-email")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> VerifyStudent(VerifyStudentCommand command)
    {
        await Sender.Send(command);

        return NoContent();
    }

    [HttpPost("register-student")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterStudent(RegisterStudentCommand command)
    {
        await Sender.Send(command);

        return NoContent();
    }

    [HttpPost("forgot-password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ForgotPassword(SendPasswordResetEmailCommand command)
    {
        await Sender.Send(command);

        return NoContent();
    }

    [HttpPost("reset-password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ResetPassword(ResetPasswordCommand command)
    {
        await Sender.Send(command);

        return NoContent();
    }

    // helper methods
    private void SetTokenCookie(string token)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddDays(7),
            SameSite = SameSiteMode.None,
            Secure = true,
        };
        Response.Cookies.Append("refreshToken", token, cookieOptions);
    }
}
