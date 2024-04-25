using System.Diagnostics.CodeAnalysis;
using Ardalis.GuardClauses;

namespace BevMan.Web.Infrastructure;

public static class IEndpointRouteBuilderExtensions
{
    public static IEndpointRouteBuilder MapGet(this IEndpointRouteBuilder builder, Delegate handler,
        [StringSyntax("Route")] string pattern = "", string role = "")
    {
        Guard.Against.AnonymousMethod(handler);

        builder.MapGet(pattern, handler)
            .WithName(handler.Method.Name)
            .AddRoleRequirement(role);

        return builder;
    }

    public static IEndpointRouteBuilder MapPost(this IEndpointRouteBuilder builder, Delegate handler,
        [StringSyntax("Route")] string pattern = "", string role = "")
    {
        Guard.Against.AnonymousMethod(handler);

        builder.MapPost(pattern, handler)
            .WithName(handler.Method.Name)
            .AddRoleRequirement(role);

        return builder;
    }

    public static IEndpointRouteBuilder MapPut(this IEndpointRouteBuilder builder, Delegate handler,
        [StringSyntax("Route")] string pattern, string role = "")
    {
        Guard.Against.AnonymousMethod(handler);

        builder.MapPut(pattern, handler)
            .WithName(handler.Method.Name)
            .AddRoleRequirement(role);

        return builder;
    }

    public static IEndpointRouteBuilder MapDelete(this IEndpointRouteBuilder builder, Delegate handler,
        [StringSyntax("Route")] string pattern, string role = "")
    {
        Guard.Against.AnonymousMethod(handler);

        builder.MapDelete(pattern, handler)
            .WithName(handler.Method.Name)
            .AddRoleRequirement(role);

        return builder;
    }

    private static RouteHandlerBuilder AddRoleRequirement(this RouteHandlerBuilder route, string? role)
    {
        if (!string.IsNullOrEmpty(role))
        {
            route.RequireAuthorization(p => p.RequireRole(role));
        }

        return route;
    }
}
