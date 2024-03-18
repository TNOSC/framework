﻿/*
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

using Tnosc.Components.Infrastructure.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Tnosc.Components.Infrastructure.Api;
/// <summary>
/// Extension methods for configuring Cross-Origin Resource Sharing (CORS) policies.
/// </summary>
public static class Extensions
{
    public const string Cors = "cors";
    /// <summary>
    /// Adds a Cross-Origin Resource Sharing (CORS) policy to the service collection based on the configuration provided.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration containing CORS settings.</param>
    /// <returns>The modified service collection.</returns>
    public static IServiceCollection AddCorsPolicy(this IServiceCollection services, IConfiguration configuration)
    {
        // Retrieve the CORS configuration section from the overall configuration
        var section = configuration.GetSection(Cors);

        // Get the CORS options from the configuration section
        var corsOptions = section.GetOptions<CorsOptions>();

        // Add CORS services to the service collection
        return services.AddCors(cors =>
        {
            // Extract allowed headers, methods, origins, and exposed headers from the options
            var allowedHeaders = corsOptions.AllowedHeaders ?? Enumerable.Empty<string>();
            var allowedMethods = corsOptions.AllowedMethods ?? Enumerable.Empty<string>();
            var allowedOrigins = corsOptions.AllowedOrigins ?? Enumerable.Empty<string>();
            var exposedHeaders = corsOptions.ExposedHeaders ?? Enumerable.Empty<string>();

            // Add a CORS policy named "cors"
            cors.AddPolicy(Cors, corsBuilder =>
            {
                // Convert allowed origins to an array
                var origins = allowedOrigins.ToArray();

                // Allow or disallow credentials based on configuration
                if (corsOptions.AllowCredentials && origins.FirstOrDefault() != "*")
                {
                    corsBuilder.AllowCredentials();
                }
                else
                {
                    corsBuilder.DisallowCredentials();
                }

                // Configure allowed headers, methods, origins, and exposed headers
                corsBuilder.WithHeaders(allowedHeaders.ToArray())
                    .WithMethods(allowedMethods.ToArray())
                    .WithOrigins(origins.ToArray())
                    .WithExposedHeaders(exposedHeaders.ToArray());
            });
        });
    }
}
