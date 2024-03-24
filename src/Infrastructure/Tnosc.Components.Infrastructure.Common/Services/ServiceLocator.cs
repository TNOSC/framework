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

using Microsoft.Extensions.DependencyInjection;

namespace Tnosc.Components.Infrastructure.Common.Services;
/// <summary>
/// Provides a centralized access point for retrieving services from the service provider.
/// </summary>
public static class ServiceLocator
{
    private static IServiceProvider _serviceProvider = null!;

    /// <summary>
    /// Sets the service provider instance to be used for service retrieval.
    /// </summary>
    /// <param name="serviceProvider">The service provider instance to be set.</param>
    public static void SetServiceProvider(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Retrieves a service of the specified type from the service provider.
    /// </summary>
    /// <typeparam name="T">The type of service to retrieve.</typeparam>
    /// <returns>The service instance of type <typeparamref name="T"/>.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the service provider has not been set or if the requested service is not available.
    /// </exception>
    public static T GetService<T>()
    {
        if (_serviceProvider == null)
        {
            throw new InvalidOperationException("The service provider has not been set.");
        }

        var service = _serviceProvider.GetService<T>();
        if (service == null)
        {
            throw new InvalidOperationException($"Service of type '{typeof(T).FullName}' is not registered.");
        }

        return service;
    }
}

