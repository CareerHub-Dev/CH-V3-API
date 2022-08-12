using Application.Auth.Commands;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using WebUI.DTO.Auth;

namespace WebUI.Controllers;

public class AccountsController : ApiControllerBase
{
    private readonly IValidator<AuthenticateRequest> _authenticateRequestValidator;

    public AccountsController(IValidator<AuthenticateRequest> authenticateRequestValidator)
    {
        _authenticateRequestValidator = authenticateRequestValidator;
    }

    [HttpPost("authenticate-{clientType}")]
    public async Task<IActionResult> Authenticate(AuthenticateRequest authenticateRequest, string clientType)
    {
        var validationResult = await _authenticateRequestValidator.ValidateAsync(authenticateRequest);

        if (!validationResult.IsValid)
        {
            return BadRequest(new ValidationProblemDetails(validationResult.ToDictionary())
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
            });
        }

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
