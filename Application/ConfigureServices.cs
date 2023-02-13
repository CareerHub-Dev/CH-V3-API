using Application.Accounts.Queries.Authenticate;
using Application.BackgroundServices;
using Application.Common.Behaviours;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Models.Email;
using Application.Helpers;
using Application.Services;
using Application.Services.Jwt;
using Application.Services.StudentInfoParser;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;

namespace Application;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddValidatorsFromAssembly(typeof(AuthenticateQuery).Assembly);
        ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo("en-US");

        services.AddMediatR(typeof(AuthenticateQuery).Assembly);

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));

        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IEmailTemplateParserService, EmailTemplateParserService>();
        services.AddScoped<IEmailTemplatesService, EmailTemplatesService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IImagesService, ImagesService>();
        services.AddScoped<IPasswordHasher<Account>, BCryptPasswordHasher<Account>>();

        services.AddScoped<IAccountHelper, AccountHelper>();
        services.AddScoped<IRefreshTokenHelper, RefreshTokenHelper>();
        services.AddScoped<IStudentInfoParserService, StudentInfoParserService>();

        services.AddHostedService<RemoveOldImagesBackgroundService>();

        services.Configure<EmailTemplateSettings>(
            configuration.GetSection(nameof(EmailTemplateSettings)));

        services.Configure<EmailSettings>(
            configuration.GetSection(nameof(EmailSettings)));

        services.Configure<ClientSettings>(
            configuration.GetSection(nameof(ClientSettings)));

        services.Configure<JwtSettings>(
            configuration.GetSection(nameof(JwtSettings)));

        return services;
    }
}
