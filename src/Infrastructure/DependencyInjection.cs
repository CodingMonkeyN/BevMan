using System.Text;
using BevMan.Application.Common.Interfaces;
using BevMan.Domain.Constants;
using BevMan.Domain.Entities;
using BevMan.Infrastructure.Data;
using BevMan.Infrastructure.Data.Interceptors;
using BevMan.Infrastructure.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using Supabase;
using SupabaseOptions = BevMan.Infrastructure.Models.SupabaseOptions;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        string? connectionString = configuration.GetConnectionString("DefaultConnection");

        Guard.Against.Null(connectionString, message: "Connection string 'DefaultConnection' not found.");

        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();
        services.AddOptionsWithValidateOnStart<SupabaseOptions>("supabase");

        NpgsqlDataSourceBuilder dataSourceBuilder = new(connectionString);
        dataSourceBuilder.MapEnum<AppRole>();
        NpgsqlDataSource dataSource = dataSourceBuilder.Build();
        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());

            options.UseNpgsql(dataSource)
                .UseSnakeCaseNamingConvention();
        });

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        services.AddSingleton(TimeProvider.System);

        services.AddSupabase();
        using (IServiceScope scope = services.BuildServiceProvider().CreateScope())
        {
            ApplicationDbContext db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            db.Database.Migrate();
        }

        services.AddAuthorization(options =>
            options.AddPolicy(Policies.CanPurge, policy => policy.RequireRole(Roles.Administrator)));

        return services;
    }

    private static IServiceCollection AddSupabase(this IServiceCollection services)
    {
        services.AddOptions<SupabaseOptions>()
            .BindConfiguration("Supabase")
            .ValidateDataAnnotations()
            .ValidateOnStart();


        ServiceProvider serviceProvider = services.BuildServiceProvider();

        SupabaseOptions supabaseOptions = serviceProvider.GetRequiredService<IOptions<SupabaseOptions>>().Value;
        SymmetricSecurityKey supasbaseSignatureKey = new(Encoding.UTF8.GetBytes(supabaseOptions.JwtSecret));
        string validIssuer = $"https://{supabaseOptions.ProjectName}.supabase.co/auth/v1";
        string[] validAudiences = { "authenticated" };

        services.AddAuthentication().AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = supasbaseSignatureKey,
                ValidAudiences = validAudiences,
                ValidIssuer = validIssuer
            };
            options.TokenValidationParameters.RoleClaimType = "app_roles";
        });

        services.AddScoped<IStorageService, SupabaseStorageService>();
        services.AddScoped<Client>(_ => new Client(supabaseOptions.ProjectUrl, supabaseOptions.ApiKey));
        return services;
    }
}
