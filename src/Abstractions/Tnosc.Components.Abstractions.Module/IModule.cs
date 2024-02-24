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

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;

namespace Tnosc.Components.Abstractions.Module;
/// <summary>
/// Represents a modular component in the application.
/// </summary>
public interface IModule
{
    /// <summary>
    /// Gets the name of the module.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets the policies associated with the module.
    /// </summary>
    /// <remarks>
    /// Implementers can provide custom policies. Defaults to null if not overridden.
    /// </remarks>
    IEnumerable<string>? Policies => null;

    /// <summary>
    /// Registers services related to the module.
    /// </summary>
    /// <param name="services">The service collection to register services.</param>
    /// <param name="configuration">The configuration related to the module.</param>
    void Register(IServiceCollection services, IConfiguration configuration);

    /// <summary>
    /// Configures middleware components for the module.
    /// </summary>
    /// <param name="app">The application builder to configure middleware.</param>
    void Use(IApplicationBuilder app);
}