using API.Authorize;
using API.DTO.Requests.RefreshTokens;
using Application.Accounts.Commands.RegisterStudent;
using Application.Accounts.Commands.ResetPassword;
using Application.Accounts.Commands.VerifyAdminWithContinuedRegistration;
using Application.Accounts.Commands.VerifyCompanyWithContinuedRegistration;
using Application.Accounts.Commands.VerifyStudent;
using Application.Accounts.Queries.Authenticate;
using Application.Accounts.Queries.RefreshToken;
using Application.Common.Interfaces;
using Application.Emails.Commands.SendPasswordResetEmail;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using OneSignalApi.Api;
using OneSignalApi.Client;
using OneSignalApi.Model;
using System.IO;

namespace API.Areas;

[Route("api/[controller]")]
public class AccountController : ApiControllerBase
{
    private readonly INotificationService _notificationService;

    public AccountController(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

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

    [HttpGet("test-send")]
    public async Task<IActionResult> Test([FromQuery] Guid playerId, [FromQuery] string webUrl, [FromQuery] string enMessage, [FromQuery] string ukMessage, [FromQuery] string largeIcon)
    {
        await _notificationService.SendNotificationAsync(new List<Guid> { playerId }, webUrl, enMessage, ukMessage, largeIcon);

        return Ok();
    }

    private static readonly string WordProcessingMlFormat = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";

    [HttpGet("cv-test")]
    public IActionResult Test([FromServices] ICVWordGenerator wordGenerator)
    {
        return File(wordGenerator.GenerateDocument(Default), WordProcessingMlFormat, "my-cv.docx");
    }

    public static readonly CV Default = new()
    {
        FirstName = "FirstName",
        LastName = "LastName",
        Photo = null,
        Goals = "I am seeking a position as a JavaScript trainee, to leverage my skills and passion for learning to make interesting and useful projects in a team of professionals.",
        HardSkills = new()
        {
            "Python", "Mainframe", "TypeScript",
            "UML", "Spring", "Next.js", "Docker", "Kubernetes", "Git",
            "Rust", "Flutter", "Dart"
        },
        SoftSkills = new()
        {
            "Problem Solving",
            "Leadership",
            "Cooperation",
            "Hard-working",
            "Empathy",
            "Good manager",
        },
        ForeignLanguages = new()
        {
            new()
            {
                LanguageLevel = LanguageLevel.B2,
                Name = "English"
            },
            new()
            {
                LanguageLevel = LanguageLevel.A2,
                Name = "German"
            },
            new()
            {
                LanguageLevel = LanguageLevel.B1,
                Name = "Polish"
            },
        },
        Experiences = new()
        {
            new()
            {
                Title = "Python Developer",
                CompanyName = "DataArt",
                StartDate = new DateTime(2020, 9, 1),
                EndDate = new DateTime(2021, 1, 1)
            },
            new()
            {
                Title = "Software Development Engineer in Test",
                CompanyName = "Rocket Software",
                StartDate = new DateTime(2021, 6, 1)
            },
        },
        ProjectLinks = new()
        {
            new() { Title = "Graphical SSH client", Url = "https://github.com/oxodao/sshelter-ui" },
            new() { Title = "i3-keyboard-layout-switcher", Url = "https://github.com/porras/i3-keyboard-layout" },
            new() { Title = "AMD-6850-Rrembrandt Driver", Url = "https://github.com/GPUOpen-Drivers/AMDVLK" },
            new() { Title = "vim-together", Url = "https://github.com/ThePrimeagen/vim-with-me" },
        },
        Educations = new()
        {
            new()
            {
                Degree = Degree.Master,
                Specialty = "Computer Science",
                University = "University of Warsaw",
                Country = "Poland",
                City = "Warsaw",
                StartDate = new DateTime(2017, 04, 22),
                EndDate = new DateTime(2020, 04, 22)
            },
            new()
            {
                Degree = Degree.Bachelor,
                Specialty = "Computer Science",
                University = "University of Warsaw",
                Country = "Poland",
                City = "Warsaw",
                StartDate = new DateTime(2013, 04, 22),
                EndDate = new DateTime(2017, 04, 22)
            },
        }
    };

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
