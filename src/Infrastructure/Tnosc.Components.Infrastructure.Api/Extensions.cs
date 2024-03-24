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

using Tnosc.Components.Infrastructure.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tnosc.Components.Abstractions.Common.Results;
using Tnosc.Components.Infrastructure.Common.Results;

namespace Tnosc.Components.Infrastructure.Api;
/// <summary>
/// Extension methods for configuring Cross-Origin Resource Sharing (CORS) policies.
/// </summary>
public static class Extensions
{
    public const string CorsSectionName = "cors";
    /// <summary>
    /// Adds a Cross-Origin Resource Sharing (CORS) policy to the service collection based on the configuration provided.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration containing CORS settings.</param>
    /// <returns>The modified service collection.</returns>
    public static IServiceCollection AddCorsPolicy(this IServiceCollection services, IConfiguration configuration)
    {
        // Retrieve the CORS configuration section from the overall configuration
        var section = configuration.GetSection(CorsSectionName);

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
            cors.AddPolicy(CorsSectionName, corsBuilder =>
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

    /// <summary>
    /// Matches the result and returns the output based on whether the operation is successful or not.
    /// </summary>
    /// <typeparam name="TOut">The type of output to return.</typeparam>
    /// <param name="result">The result to match.</param>
    /// <param name="onSuccess">The function to execute if the operation is successful.</param>
    /// <param name="onFailure">The function to execute if the operation fails.</param>
    /// <returns>The output returned by either <paramref name="onSuccess"/> or <paramref name="onFailure"/> based on the result.</returns>
    public static TOut Match<TOut>(
        this Result result,
        Func<TOut> onSuccess,
        Func<Result, TOut> onFailure)
    {
        return result.IsSuccess ? onSuccess() : onFailure(result);
    }

    /// <summary>
    /// Matches the result with a value and returns the output based on whether the operation is successful or not.
    /// </summary>
    /// <typeparam name="TIn">The type of value contained in the result.</typeparam>
    /// <typeparam name="TOut">The type of output to return.</typeparam>
    /// <param name="result">The result to match.</param>
    /// <param name="onSuccess">The function to execute if the operation is successful, taking the result value as input.</param>
    /// <param name="onFailure">The function to execute if the operation fails, taking the result as input.</param>
    /// <returns>The output returned by either <paramref name="onSuccess"/> or <paramref name="onFailure"/> based on the result.</returns>
    public static TOut Match<TIn, TOut>(
        this Result<TIn> result,
        Func<TIn, TOut> onSuccess,
        Func<Result<TIn>, TOut> onFailure)
    {
        return result.IsSuccess ? onSuccess(result.Value) : onFailure(result);
    }

}