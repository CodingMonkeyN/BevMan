using BevMan.Application.Common.Interfaces;
using BevMan.Infrastructure.Data;
using BevMan.Web.Infrastructure;
using BevMan.Web.OpenApi;
using BevMan.Web.Services;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;

namespace BevMan.Web;

public static class DependencyInjection
{
    public static IServiceCollection AddWebServices(this IServiceCollection services)
    {
        services.AddScoped<IUser, CurrentUser>();

        services.AddHttpContextAccessor();

        services.AddHealthChecks()
            .AddDbContextCheck<ApplicationDbContext>();

        services.AddExceptionHandler<CustomExceptionHandler>();

        services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(config =>
        {
            config.SchemaFilter<RequiredMemberFilter>();
        });
        services.AddFluentValidationRulesToSwagger();

        return services;
    }

    public static WebApplicationBuilder SetupConfiguration(this WebApplicationBuilder builder)
    {
        string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
        builder.Configuration
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false, true)
            .AddJsonFile($"appsettings.{env}.json", true, true)
            .AddEnvironmentVariables();
        return builder;
    }
}
