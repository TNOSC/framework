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

namespace Tnosc.Components.Abstractions.Context;

/// <summary>
/// Represents the context information for a request or an execution context in the application.
/// </summary>
public interface IContext
{
    /// <summary>
    /// Gets the unique identifier for the current request.
    /// </summary>
    Guid RequestId { get; }

    /// <summary>
    /// Gets the unique identifier used for correlating related activities.
    /// </summary>
    Guid CorrelationId { get; }

    /// <summary>
    /// Gets the trace identifier for distributed tracing.
    /// </summary>
    string? TraceId { get; }

    /// <summary>
    /// Gets the IP address associated with the current request.
    /// </summary>
    string? IpAddress { get; }

    /// <summary>
    /// Gets the user agent string associated with the current request.
    /// </summary>
    string? UserAgent { get; }

    /// <summary>
    /// Gets the identity context associated with the current request.
    /// </summary>
    IIdentityContext? Identity { get; }
}
