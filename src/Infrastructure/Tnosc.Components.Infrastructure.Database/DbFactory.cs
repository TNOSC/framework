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
using Tnosc.Components.Abstractions.Common;
using Tnosc.Components.Abstractions.Database.Common;

namespace Tnosc.Components.Infrastructure.Database;
/// <summary>
/// Factory class for creating and managing instances of a specific DbContext.
/// </summary>
/// <typeparam name="TDbContext">The type of the DbContext to be created.</typeparam>
public class DbFactory<TDbContext> : Disposable, IDbFactory<TDbContext>
    where TDbContext : DbContext
{
    private readonly IServiceProvider _serviceProvider;
    private TDbContext? _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="DbFactory{TDbContext}"/> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider used for creating DbContext instances.</param>
    public DbFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    /// <inheritdoc />
    public TDbContext GetDbContext()
    {
        // If an instance of DbContext already exists, return it
        if (_dbContext is not null)
            return _dbContext;

        // Create a new instance of DbContext using the service provider
        _dbContext = _serviceProvider.GetService(typeof(TDbContext)) as TDbContext;

        // If unable to create a DbContext instance, throw an exception
        if (_dbContext is null)
            throw new InvalidCastException("Failed to cast DbContext from the service provider.");

        return _dbContext;
    }

   /// <inheritdoc />
    protected override void DisposeCore()
    {
        // Dispose of the DbContext instance if it exists
        _dbContext?.Dispose();
    }
}
