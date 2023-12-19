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
using Tnosc.Components.Abstractions.Database.Common;

namespace Tnosc.Components.Abstractions.Database;
/// <summary>
/// Base class for implementing a command-specific repository that supports CRUD operations.
/// </summary>
/// <typeparam name="TDbContext">The type of DbContext associated with the repository.</typeparam>
/// <typeparam name="TEntity">The type of entity handled by the repository.</typeparam>
/// <typeparam name="TKey">The type of the entity's primary key.</typeparam>
public class CommandRepositoryBase<TDbContext, TEntity, TKey> : RepositoryBase<TDbContext, TEntity, TKey>, ICommandRepository<TEntity, TKey>
    where TDbContext : DbContext
    where TEntity : class
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CommandRepositoryBase{TDbContext, TEntity, TKey}"/> class.
    /// </summary>
    /// <param name="dbFactory">The factory for creating DbContext instances.</param>
    protected CommandRepositoryBase(IDbFactory<TDbContext> dbFactory)
        : base(dbFactory)
    {
    }

    /// <inheritdoc />
    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await Context.Set<TEntity>().AddAsync(entity, cancellationToken);
    }

    /// <inheritdoc />
    public void Update(TEntity entity)
    {
        Context.Set<TEntity>().Attach(entity);
        Context.Entry(entity).State = EntityState.Modified;
    }

    /// <inheritdoc />
    public void Delete(TEntity entity)
    {
        Context.Set<TEntity>().Remove(entity);
    }

    /// <inheritdoc />
    public async override Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
    {
        return await Context.Set<TEntity>().FindAsync(id, cancellationToken);
    }

    /// <inheritdoc />
    public async override Task<TEntity?> GetBySpecAsync(ISpecification<TEntity> spec, CancellationToken cancellationToken = default)
    {
        return await ApplySpecification(spec).FirstOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async override Task<IReadOnlyList<TEntity>> ListAllAsync(CancellationToken cancellationToken = default)
    {
        return await Context.Set<TEntity>().ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async override Task<IReadOnlyList<TEntity>> ListAsync(ISpecification<TEntity> spec, CancellationToken cancellationToken = default)
    {
        return await ApplySpecification(spec).ToListAsync(cancellationToken);
    }
}