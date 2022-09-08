using Application.Accounts.Commands.RegisterStudent;
using Application.Accounts.Commands.ResetPassword;
using Application.Accounts.Commands.VerifyAdminWithContinuedRegistration;
using Application.Accounts.Commands.VerifyCompanyWithContinuedRegistration;
using Application.Accounts.Commands.VerifyStudent;
using Application.Accounts.Queries.Authenticate;
using Application.Accounts.Queries.RefreshToken;
using Application.Emails.Commands;
using Microsoft.AspNetCore.Mvc;
using WebUI.Common.Models.Account;

namespace WebUI.Areas;

[Route("api/[controller]")]
public class AccountsController : ApiControllerBase
{
    [HttpPost("authenticate-{clientType}")]
    public async Task<IActionResult> Authenticate(AuthenticateQuery query, string clientType)
    {
        var response = await Mediator.Send(query);

        switch (clientType)
        {
            case "web":
                {
                    SetTokenCookie(response.RefreshToken);
                    return Ok(new
                    {
                        response.AccountId,
                        response.JwtToken,
                        response.JwtTokenExpires,
                        response.Role
                    });
                }
            default:
                return Ok(response);
        }
    }

    [HttpPost("refresh-token-{clientType}")]
    public async Task<IActionResult> RefreshToken(RefreshTokenView view, string clientType)
    {
        if (string.IsNullOrWhiteSpace(view.Token))
        {
            view.Token = Request.Cookies["refreshToken"];
        }

        if (string.IsNullOrWhiteSpace(view.Token))
        {
            return Problem(title: "Token is required.", statusCode: StatusCodes.Status400BadRequest, detail: "Body or cookies don't contain token.");
        }

        var response = await Mediator.Send(new RefreshTokenQuery
        {
            Token = view.Token
        });

        switch (clientType)
        {
            case "web":
                {
                    SetTokenCookie(response.RefreshToken);
                    return Ok(new
                    {
                        response.AccountId,
                        response.JwtToken,
                        response.JwtTokenExpires,
                        response.Role
                    });
                }
            default:
                return Ok(response);

        }
    }

    [HttpPost("verify-company-email")]
    public async Task<IActionResult> VerifyCompanyWithContinuedRegistration([FromForm] VerifyCompanyWithContinuedRegistrationCommand command)
    {
        await Mediator.Send(command);

        return Ok(new { message = "Verification successful, you can now login" });
    }
    [HttpPost("verify-admin-email")]
    public async Task<IActionResult> VerifyAdminWithContinuedRegistration(VerifyAdminWithContinuedRegistrationCommand command)
    {
        await Mediator.Send(command);

        return Ok(new { message = "Verification successful, you can now login" });
    }
    [HttpPost("verify-student-email")]
    public async Task<IActionResult> VerifyStudent(VerifyStudentCommand command)
    {
        await Mediator.Send(command);

        return Ok(new { message = "Verification successful, you can now login" });
    }

    [HttpPost("register/student")]
    public async Task<IActionResult> RegisterStudent(RegisterStudentCommand command)
    {
        await Mediator.Send(command);

        return Ok(new { message = "Registration successful, please check your email for verification instructions" });
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword(SendPasswordResetEmailCommand command)
    {
        await Mediator.Send(command);

        return Ok(new { message = "Please check your email for password reset instructions" });
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(ResetPasswordCommand command)
    {
        await Mediator.Send(command);

        return Ok(new { message = "Password reset successful, you can now login" });
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
