using Application.Common.Behaviours;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Services;
using Application.Services.Jwt;
using Application.Services.Procedure;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));

        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IProcedureService, ProcedureService>();
        services.AddScoped<IEmailTemplateParserService, EmailTemplateParserService>();
        services.AddScoped<IEmailTemplatesService, EmailTemplatesService>();

        services.Configure<AppSettings>(configuration.GetSection("AppSettings"));

        return services;
    }
}
