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
using Tnosc.Components.Abstractions.Context;

namespace Tnosc.Components.Infrastructure.Context;

/// <summary>
/// Represents the context information for a request or an execution context in the application.
/// </summary>
public class Context : IContext
{
    /// <summary>
    /// Gets the unique identifier for the current request.
    /// </summary>
    public Guid RequestId { get; } = Guid.NewGuid();

    /// <summary>
    /// Gets the unique identifier used for correlating related activities.
    /// </summary>
    public Guid CorrelationId { get; }

    /// <summary>
    /// Gets the trace identifier for distributed tracing.
    /// </summary>
    public string? TraceId { get; }

    /// <summary>
    /// Gets the IP address associated with the current request.
    /// </summary>
    public string? IpAddress { get; }

    /// <summary>
    /// Gets the user agent string associated with the current request.
    /// </summary>
    public string? UserAgent { get; }

    /// <summary>
    /// Gets the identity context associated with the current request.
    /// </summary>
    public IIdentityContext? Identity { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Context"/> class with default values.
    /// </summary>
    public Context() 
        : this(Guid.NewGuid(), $"{Guid.NewGuid():N}", null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Context"/> class based on an <see cref="HttpContext"/>.
    /// </summary>
    /// <param name="context">The HttpContext representing the current request context.</param>
    public Context(HttpContext context) 
        : this(context.TryGetCorrelationId(), context.TraceIdentifier,
        new IdentityContext(context.User), context.GetUserIpAddress(),
        context.Request.Headers["user-agent"])
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Context"/> class with specified values.
    /// </summary>
    /// <param name="correlationId">The correlation identifier for the context.</param>
    /// <param name="traceId">The trace identifier for distributed tracing.</param>
    /// <param name="identity">The identity context associated with the context.</param>
    /// <param name="ipAddress">The IP address associated with the context.</param>
    /// <param name="userAgent">The user agent string associated with the context.</param>
    public Context(Guid? correlationId, string traceId, IIdentityContext? identity = null, string? ipAddress = null,
        string? userAgent = null)
    {
        // Set properties based on the provided values or default values
        CorrelationId = correlationId ?? Guid.NewGuid();
        TraceId = traceId;
        Identity = identity ?? IdentityContext.Empty;
        IpAddress = ipAddress;
        UserAgent = userAgent;
    }

    /// <summary>
    /// Gets an empty instance of <see cref="Context"/>.
    /// </summary>
    public static IContext Empty => new Context();
}


