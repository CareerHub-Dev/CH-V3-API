using Application.Accounts.Commands.AccountOwnsToken;
using Application.Accounts.Commands.Authenticate;
using Application.Accounts.Commands.ChangePassword;
using Application.Accounts.Commands.RefreshToken;
using Application.Accounts.Commands.RegisterStudent;
using Application.Accounts.Commands.ResetPassword;
using Application.Accounts.Commands.RevokeToken;
using Application.Accounts.Commands.VerifyAdminWithContinuedRegistration;
using Application.Accounts.Commands.VerifyCompanyWithContinuedRegistration;
using Application.Accounts.Commands.VerifyStudent;
using Application.Accounts.Query;
using Application.Emails.Commands;
using Microsoft.AspNetCore.Mvc;
using WebUI.Authorize;
using WebUI.Common.Extentions;
using WebUI.Common.Models.Account;

namespace WebUI.Controllers;

public class AccountController : ApiControllerBase
{
    [HttpPost("authenticate-{clientType}")]
    public async Task<IActionResult> Authenticate(AuthenticateRequest authenticate, string clientType)
    {
        var response = await Mediator.Send(new AuthenticateCommand
        {
            Email = authenticate.Email,
            Password = authenticate.Password,
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

    [HttpPost("refresh-token-{clientType}")]
    public async Task<IActionResult> RefreshToken(RefreshTokenRequest refreshToken, string clientType)
    {
        if (string.IsNullOrWhiteSpace(refreshToken.Token))
        {
            refreshToken.Token = Request.Cookies["refreshToken"];
        }

        if (string.IsNullOrWhiteSpace(refreshToken.Token))
        {
            return Problem(title: "Bad Request", statusCode: StatusCodes.Status400BadRequest, detail: "Token is required");
        }

        var response = await Mediator.Send(new RefreshTokenCommand
        {
            Token = refreshToken.Token,
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
    public async Task<IActionResult> VerifyCompanyWithContinuedRegistration([FromForm] VerifyCompanyWithContinuedRegistrationRequest verifyCompany)
    {
        var companyInfo = new VerifyCompanyWithContinuedRegistrationCommand
        {
            Token = verifyCompany.Token,
            Logo = verifyCompany.LogoFile is IFormFile logo ? await logo.ToCreateImageAsync() : null,
            Banner = verifyCompany.BannerFile is IFormFile banner ? await banner.ToCreateImageAsync() : null,
            Name = verifyCompany.Name,
            Motto = verifyCompany.Motto,
            Description = verifyCompany.Description,
            Password = verifyCompany.Password,
        };

        await Mediator.Send(companyInfo);
        return Ok(new { message = "Verification successful, you can now login" });
    }
    [HttpPost("verify-admin-email")]
    public async Task<IActionResult> VerifyAdminWithContinuedRegistration([FromForm] VerifyAdminWithContinuedRegistrationRequest verifyAdmin)
    {
        var adminInfo = new VerifyAdminWithContinuedRegistrationCommand
        {
            Token = verifyAdmin.Token,
            Password = verifyAdmin.Password,
        };

        await Mediator.Send(adminInfo);
        return Ok(new { message = "Verification successful, you can now login" });
    }
    [HttpPost("verify-student-email")]
    public async Task<IActionResult> VerifyStudent([FromForm] VerifyStudentRequest verifyStudent)
    {
        await Mediator.Send(new VerifyStudentCommand { Token = verifyStudent.Token });
        return Ok(new { message = "Verification successful, you can now login" });
    }

    [HttpPost("register/student")]
    public async Task<IActionResult> RegisterStudent(RegisterStudentRequest registerStudent)
    {
        await Mediator.Send(new RegisterStudentCommand { Email = registerStudent.Email, Password = registerStudent.Password });

        return Ok(new { message = "Registration successful, please check your email for verification instructions" });
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
    
    /// <summary>
    /// Auth
    /// </summary>
    [Authorize]
    [HttpPost("revoke-token")]
    public async Task<IActionResult> RevokeTokenAsync(RevokeTokenRequest revokeToken)
    {
        if (string.IsNullOrWhiteSpace(revokeToken.Token))
        {
            revokeToken.Token = Request.Cookies["refreshToken"];
        }

        if (string.IsNullOrWhiteSpace(revokeToken.Token))
        {
            return Problem(title: "Bad Request", statusCode: StatusCodes.Status400BadRequest, detail: "Token is required");
        }

        if(AccountInfo!.Role != "Admin" && !await Mediator.Send(new AccountOwnsTokenCommand { Token = revokeToken.Token, AccountId = AccountInfo!.Id }))
        {
            return Problem(title: "Not Found", statusCode: StatusCodes.Status404NotFound, detail: "Token is not found");
        }

        await Mediator.Send(new RevokeTokenCommand { Token = revokeToken.Token, IpAddress = IpAddress() });

        return Ok(new { message = "Token revoked" });
    }

    /// <summary>
    /// Auth
    /// </summary>
    [Authorize]
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword(ChangePasswordRequest changePassword)
    {
        await Mediator.Send(new ChangePasswordCommand {
            OldPassword = changePassword.OldPassword,
            NewPassword = changePassword.NewPassword,
            AccountId = AccountInfo!.Id,
        });

        return Ok(new { message = "Password change successful" });
    }

    /// <summary>
    /// Admin
    /// </summary>
    [Authorize("Admin")]
    [HttpGet("{accountId}")]
    public async Task<AccountBriefResponse> GetAccountBrief(Guid accountId)
    {
        var result = await Mediator.Send(new GetAccountBriefQuery(accountId));

        return new AccountBriefResponse(result);
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
