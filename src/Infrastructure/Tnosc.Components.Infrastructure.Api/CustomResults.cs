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

using Microsoft.AspNetCore.Http;
using Tnosc.Components.Abstractions.Common.Results;
using Tnosc.Components.Abstractions.Context;
using Tnosc.Components.Infrastructure.Common.Results;
using Tnosc.Components.Infrastructure.Common.Services;

namespace Tnosc.Components.Infrastructure.Api;
/// <summary>
/// Provides methods to generate problem details for various types of results.
/// </summary>
public static class CustomResults
{
    /// <summary>
    /// Generates a problem detail response based on the provided result.
    /// </summary>
    /// <param name="result">The result to generate the problem detail for.</param>
    /// <returns>An <see cref="Microsoft.AspNetCore.Http.IResult"/> representing the problem detail.</returns>
    public static Microsoft.AspNetCore.Http.IResult Problem(Result result)
    {
        if (result.IsSuccess)
        {
            throw new InvalidOperationException("Cannot generate problem detail for successful result.");
        }

        return Results.Problem(
            title: GetTitle(result.Error.Type),
            detail: GetDetail(result),
            type: GetType(result.Error.Type),
            statusCode: GetStatusCode(result.Error.Type),
            instance: ServiceLocator.GetService<IHttpContextAccessor>().HttpContext!.Request.Path,
            extensions: GetExtensions(result));
    }

    private static string GetTitle(ErrorType errorType)
    {
        return errorType switch
        {
            ErrorType.Validation => "Bad Request",
            ErrorType.NotFound => "Not Found",
            ErrorType.Unauthorized => "Unauthorized",
            ErrorType.Forbidden => "Forbidden",
            ErrorType.Conflict => "Conflict",
            _ => "Internal Server Error"
        };
    }

    private static string GetDetail(Result result) =>
        result.Error.Type switch
        {
            ErrorType.Validation => result is not ValidationResult ? result.Error.Description : "One or more validations errors",
            ErrorType.NotFound => result.Error.Description,
            ErrorType.Unauthorized => result.Error.Description,
            ErrorType.Forbidden => result.Error.Description,
            ErrorType.Conflict => result.Error.Description,
            _ => "An unexpected error occurred"
        };

    private static string GetType(ErrorType errorType) =>
        errorType switch
        {
            ErrorType.Validation => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            ErrorType.NotFound => "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            ErrorType.Unauthorized => "https://tools.ietf.org/html/rfc7235#section-3.1",
            ErrorType.Forbidden => "https://tools.ietf.org/html/rfc7231#section-6.5.3",
            ErrorType.Conflict => "https://tools.ietf.org/html/rfc7231#section-6.5.8",
            _ => "https://tools.ietf.org/html/rfc7231#section-6.6.1"
        };

    private static int GetStatusCode(ErrorType errorType) =>
        errorType switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };

    private static Dictionary<string, object?>? GetExtensions(Result result)
    {
        var extensions = new Dictionary<string, object?>()
        {
            { "traceId", ServiceLocator.GetService<IContext>().TraceId }
        };

        if (result is ValidationResult validationResult)
        {
            extensions.Add("errors", validationResult.ValidationErrors);
        }

        return extensions;
    }
}


