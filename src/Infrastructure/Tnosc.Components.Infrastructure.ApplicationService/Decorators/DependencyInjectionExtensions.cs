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
using Tnosc.Components.Abstractions.ApplicationService.Commands;
using Tnosc.Components.Abstractions.ApplicationService.Queries;

namespace Tnosc.Components.Infrastructure.ApplicationService.Decorators;
/// <summary>
/// Provides extension methods for adding validating decorators to the service collection.
/// </summary>
public static class DependencyInjectionExtensions
{
    /// <summary>
    /// Adds validating decorators to the specified service collection.
    /// </summary>
    /// <param name="services">The service collection to which the decorators will be added.</param>
    /// <returns>The modified service collection.</returns>
    public static IServiceCollection AddValidatingDecorators(this IServiceCollection services)
    {
        // Try to decorate ICommandHandler<> and IQueryHandler<> services with their respective validating decorators.
        services.TryDecorate(typeof(ICommandHandler<,>), typeof(ValidatingCommandHandlerDecorator<,>));
        services.TryDecorate(typeof(IQueryHandler<,>), typeof(ValidatingQueryHandlerDecorator<,>));

        return services;
    }
}

