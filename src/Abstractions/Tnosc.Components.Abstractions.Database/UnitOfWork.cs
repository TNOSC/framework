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

namespace Tnosc.Components.Abstractions.Database;
/// <summary>
/// Abstract implementation of the unit of work pattern for managing transactions and committing changes.
/// </summary>
/// <typeparam name="TDbContext">The type of DbContext associated with the unit of work.</typeparam>
public abstract class UnitOfWork<TDbContext> :Disposable, IUnitOfWork<TDbContext>
    where TDbContext : DbContext
{
    private readonly TDbContext _dbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="UnitOfWork{TDbContext}"/> class.
    /// </summary>
    /// <param name="dbFactory">The factory for creating DbContext instances.</param>
    public UnitOfWork(IDbFactory<TDbContext> dbFactory)
    {
        _dbContext = dbFactory.GetDbContext() ?? throw new ArgumentNullException(nameof(dbFactory));
    }

    ///<inheritdoc/>
    public async Task<int> CompleteAsync()
    {
        return await _dbContext.SaveChangesAsync();
    }

    ///<inheritdoc/>
    protected override void DisposeCore()
    {
        _dbContext.Dispose();
    }
}
