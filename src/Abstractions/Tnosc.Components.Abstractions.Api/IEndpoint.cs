/*
 Copyright (c) 2023 Ahmed HEDFI (ahmed.hedfi@gmail.com)

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

using Microsoft.AspNetCore.Routing;

namespace Tnosc.Components.Abstractions.Api;
/// <summary>
/// Represents an interface for configuring routes for an endpoint.
/// </summary>
public interface IEndpoint
{
    /// <summary>
    /// Adds routes to the specified endpoint route builder.
    /// </summary>
    /// <param name="app">The endpoint route builder.</param>
    void AddRoutes(IEndpointRouteBuilder app);
}

