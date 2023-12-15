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

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Tnosc.Components.Infrastructure.Database;
/// <summary>
/// Extension methods for configuring PostgreSQL-related services in the dependency injection container.
/// </summary>
public static class DependencyInjectionExtensions
{
    /// <summary>
    /// Adds PostgreSQL-related services to the dependency injection container.
    /// </summary>
    /// <typeparam name="T">The type of the DbContext to be configured for PostgreSQL.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
    /// <param name="configuration">The configuration containing PostgreSQL connection details.</param>
    /// <returns>The updated <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddPostgres<T>(this IServiceCollection services, IConfiguration configuration)
        where T : DbContext
    {
        // Create an instance of the PostgresOptions class and bind PostgreSQL configuration
        var options = new PostgresOptions();
        configuration.GetSection("postgres").Bind(options);

        // Add the DbContext configured for PostgreSQL using the connection string
        services.AddDbContext<T>(x => x.UseNpgsql(options.ConnectionString));

        // Return the updated service collection
        return services;
    }
}
