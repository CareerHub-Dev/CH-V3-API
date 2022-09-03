using Application.Common.Interfaces;
using FluentValidation;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using System.Globalization;
using System.Reflection;
using WebUI.Filters;
using WebUI.Services;

namespace WebUI;

public static class ConfigureServices
{
    public static IServiceCollection AddWebUIServices(this IServiceCollection services)
    {
        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddSingleton<ICurrentAccountService, CurrentAccountService>();
        services.AddScoped<IPathService, PathService>();

        services.AddHttpContextAccessor();

        services.AddHealthChecks()
            .AddDbContextCheck<ApplicationDbContext>();

        services.AddControllers(options =>
        {
            options.Filters.Add<ApiExceptionFilterAttribute>();
            options.Filters.Add<ApiValidationFilter>();
        });

        services.AddEndpointsApiExplorer();

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo("en-US");

        // Customise default API behaviour
        services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);

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
