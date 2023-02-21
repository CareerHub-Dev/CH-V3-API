using API.Services;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Hellang.Middleware.ProblemDetails;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Security.Authentication;
using System.Text.Json.Serialization;

namespace API;

public static class ConfigureServices
{
    public static IServiceCollection AddAPIServices(this IServiceCollection services)
    {
        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddSingleton<ICurrentAccountService, CurrentAccountService>();
        services.AddSingleton<IСurrentRemoteIpAddressService, СurrentRemoteIpAddressService>();
        services.AddScoped<IPathService, PathService>();
        services.AddScoped<IBaseUrlService, BaseUrlService>();

        services.AddHttpContextAccessor();

        services.AddHealthChecks()
            .AddDbContextCheck<ApplicationDbContext>();

        services.AddControllers()
            .AddJsonOptions(options =>
            {
                var enumConverter = new JsonStringEnumConverter();
                options.JsonSerializerOptions.Converters.Add(enumConverter);
            });

        services.AddEndpointsApiExplorer();

        // Customise default API behaviour
        services.Configure<ApiBehaviorOptions>(options
            => options.SuppressModelStateInvalidFilter = true);

        services.AddProblemDetails();

        services.AddSwagger();

        services.AddSignalR(options =>
        {
            options.EnableDetailedErrors = true;
        });

        return services;
    }

    private static IServiceCollection AddProblemDetails(this IServiceCollection services)
    {
        services.AddProblemDetails(setup =>
        {
            setup.IncludeExceptionDetails = (ctx, ex) =>
            {
                //var env = ctx.RequestServices.GetRequiredService<IHostEnvironment>();
                //return env.IsDevelopment() || env.IsStaging();
                return true;
            };

            setup.Map<ValidationException>(ex => new ValidationProblemDetails(ex.Errors)
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Status = StatusCodes.Status400BadRequest,
                Detail = ex.Message
            });

            setup.Map<NotFoundException>(ex => new ProblemDetails()
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                Status = StatusCodes.Status404NotFound,
                Title = "The specified resource was not found.",
                Detail = ex.Message
            });

            setup.Map<AuthenticationException>(ex => new ProblemDetails()
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Status = StatusCodes.Status400BadRequest,
                Title = "Authorization failed.",
                Detail = ex.Message
            });

            setup.Map<BanException>(ex =>
            {
                var problem = new ProblemDetails()
                {
                    Type = "https://www.rfc-editor.org/rfc/rfc7231#section-6.5.3",
                    Status = StatusCodes.Status403Forbidden,
                    Title = "Forbidden.",
                    Detail = ex.Message
                };
                problem.Extensions.Add(nameof(ex.Expires), ex.Expires);
                problem.Extensions.Add(nameof(ex.Reasone), ex.Reasone);
                return problem;
            });

            setup.Map<ArgumentException>(ex => new ProblemDetails()
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Status = StatusCodes.Status400BadRequest,
                Title = "Invalid argument.",
                Detail = ex.Message
            });

            setup.Map<ArgumentOutOfRangeException>(ex => new ProblemDetails()
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Status = StatusCodes.Status400BadRequest,
                Title = "Invalid argument.",
                Detail = ex.Message
            });

            setup.Map<FileNotFoundException>(ex => new ProblemDetails()
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Status = StatusCodes.Status400BadRequest,
                Title = "The specified resource was not found.",
                Detail = ex.Message
            });

            setup.Map<ForbiddenException>(ex => new ProblemDetails()
            {
                Type = "https://www.rfc-editor.org/rfc/rfc7231#section-6.5.3",
                Status = StatusCodes.Status403Forbidden,
                Title = "Forbidden.",
                Detail = ex.Message
            });
        });

        return services;
    }

    private static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "CareerHub", Version = "v3" });
            var jwtSecurityScheme = new OpenApiSecurityScheme
            {
                Scheme = "Bearer",
                BearerFormat = "JWT",
                Name = "JWT Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer YOUR_TOKEN')",
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            };
            options.AddSecurityDefinition("Bearer", jwtSecurityScheme);
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                { jwtSecurityScheme, Array.Empty<string>() }
            });
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });

        return services;
    }
}
