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

using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Tnosc.Components.Abstractrions.ApplicationService.Events;

namespace Tnosc.Components.Infrastructure.ApplicationService.Events;
/// <summary>
/// Extension methods for configuring dependency injection related to events.
/// </summary>
public static class DependencyInjectionExtensions
{
    /// <summary>
    /// Adds event-related services to the specified IServiceCollection.
    /// </summary>
    /// <param name="services">The IServiceCollection to add services to.</param>
    /// <param name="assemblies">The assemblies containing event handlers to be registered.</param>
    /// <returns>The modified IServiceCollection.</returns>
    public static IServiceCollection AddEvents(this IServiceCollection services, IEnumerable<Assembly> assemblies)
    {
        // Register the IEventDispatcher with a singleton lifetime.
        services.AddSingleton<IEventDispatcher, EventDispatcher>();

        // Scan assemblies for types implementing IEventHandler and register them with interfaces.
        services.Scan(scan => scan
            .FromAssemblies(assemblies)
            .AddClasses(classes => classes
                .AssignableTo(typeof(IEventHandler<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        return services;
    }
}
