/*
 Copyright (c) 2024 Ahmed HEDFI (ahmed.hedfi@gmail.com)

 This program is free software: you can redistribute it and/or modify
 it under the terms of the GNU General Public License as published by
 the Free Software Foundation, either version 3 of the License, or
 (at your option) any later version.

 This program is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 GNU General Public License for more details.

 You should have received a copy of the GNU General Public License
 along with this program. If not, see <https://www.gnu.org/licenses/>.
 */

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Tnosc.Components.Infrastructure.Context;

/// <summary>
/// Extension methods for configuring and using the context within an application.
/// </summary>
public static class Extensions
{
    private const string CorrelationIdKey = "correlation-id";
    /// <summary>
    /// Adds context-related services to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The modified service collection.</returns>
    public static IServiceCollection AddContext(this IServiceCollection services)
    {
        // Add the ContextAccessor as a singleton
        services.AddSingleton<ContextAccessor>();

        // Add a transient service to obtain the current context within a request
        services.AddTransient(sp => sp.GetRequiredService<ContextAccessor>().Context);

        return services;
    }

    /// <summary>
    /// Configures the application to use the context within each request.
    /// </summary>
    /// <param name="app">The application builder.</param>
    /// <returns>The modified application builder.</returns>
    public static IApplicationBuilder UseContext(this IApplicationBuilder app)
    {
        // Middleware to set the context for each incoming request
        app.Use((ctx, next) =>
        {
            // Obtain the ContextAccessor from the request services
            var contextAccessor = ctx.RequestServices.GetRequiredService<ContextAccessor>();

            // Set the context using a new instance of Context for each request
            contextAccessor.Context = new Context(ctx);

            // Continue to the next middleware in the pipeline
            return next();
        });

        return app;
    }

    /// <summary>
    /// Middleware to set a correlation ID for each incoming request.
    /// </summary>
    /// <param name="app">The application builder.</param>
    /// <returns>The modified application builder.</returns>
    public static IApplicationBuilder UseCorrelationId(this IApplicationBuilder app)
        => app.Use((ctx, next) =>
        {
            // Add a new correlation ID to the context items
            ctx.Items.Add(CorrelationIdKey, Guid.NewGuid());
            return next();
        });

    /// <summary>
    /// Tries to retrieve the correlation ID from the HttpContext items.
    /// </summary>
    /// <param name="context">The HttpContext.</param>
    /// <returns>The correlation ID if found; otherwise, null.</returns>
    public static Guid? TryGetCorrelationId(this HttpContext context)
        => context.Items.TryGetValue(CorrelationIdKey, out var id) ? (Guid)id : (Guid?)null;

    /// <summary>
    /// Retrieves the user's IP address from the HttpContext.
    /// </summary>
    /// <param name="context">The HttpContext.</param>
    /// <returns>The user's IP address as a string.</returns>
    public static string GetUserIpAddress(this HttpContext context)
    {
        if (context is null)
        {
            return string.Empty;
        }

        var ipAddress = context.Connection.RemoteIpAddress?.ToString();
        if (context.Request.Headers.TryGetValue("x-forwarded-for", out var forwardedFor))
        {
            var ipAddresses = forwardedFor.ToString().Split(",", StringSplitOptions.RemoveEmptyEntries);
            if (ipAddresses.Any())
            {
                ipAddress = ipAddresses[0];
            }
        }

        return ipAddress ?? string.Empty;
    }
}