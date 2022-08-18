using Application.Auth.Commands.Authenticate;
using Application.Auth.Commands.ResetPassword;
using Application.Auth.Commands.VerifyCompanyWithContinuedRegistration;
using Application.Emails.Commands;
using Microsoft.AspNetCore.Mvc;
using WebUI.DTO.Auth;

namespace WebUI.Controllers;

public class AccountController : ApiControllerBase
{
    [HttpPost("authenticate-{clientType}")]
    public async Task<IActionResult> Authenticate(AuthenticateRequest authenticateRequest, string clientType)
    {
        var response = await Mediator.Send(new AuthenticateCommand
        {
            Email = authenticateRequest.Email,
            Password = authenticateRequest.Password,
            IpAddress = IpAddress()
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
    public async Task<IActionResult> VerifyCompanyEmail([FromForm] VerifyCompanyWithContinuedRegistrationRequest verifyCompanyRequest)
    {
        var companuInfo = new VerifyCompanyWithContinuedRegistrationCommand
        {
            Token = verifyCompanyRequest.Token,
            CompanyName = verifyCompanyRequest.CompanyName,
            CompanyMotto = verifyCompanyRequest.CompanyMotto,
            CompanyDescription = verifyCompanyRequest.CompanyDescription,
            Password = verifyCompanyRequest.Password,
        };

        await Mediator.Send(companuInfo);
        return Ok(new { message = "Verification successful, you can now login" });
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest forgotPassword)
    {
        await Mediator.Send(new SendPasswordResetEmailCommand { Email = forgotPassword.Email });

        return Ok(new { message = "Please check your email for password reset instructions" });
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(ResetPasswordRequest resetPassword)
    {
        await Mediator.Send(new ResetPasswordCommand { Token = resetPassword.Token, Password = resetPassword.Password });

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

    private string IpAddress()
    {
        if (Request.Headers.ContainsKey("X-Forwarded-For"))
        {
            return Request.Headers["X-Forwarded-For"];
        }
        else
        {
            return HttpContext.Connection.RemoteIpAddress!.MapToIPv4().ToString();
        }
    }
}
