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
/// Abstract base class for implementing a repository with common CRUD operations.
/// </summary>
/// <typeparam name="TDbContext">The type of DbContext associated with the repository.</typeparam>
/// <typeparam name="TEntity">The type of entity handled by the repository.</typeparam>
/// <typeparam name="TKey">The type of the entity's primary key.</typeparam>
public abstract class RepositoryBase<TDbContext, TEntity, TKey> : IRepository<TEntity, TKey>
    where TDbContext : DbContext
    where TEntity : class
{
    /// <summary>
    /// The DbContext associated with the repository.
    /// </summary>
    protected readonly TDbContext Context;

    /// <summary>
    /// Initializes a new instance of the <see cref="RepositoryBase{TDbContext, TEntity, TKey}"/> class.
    /// </summary>
    /// <param name="dbFactory">The factory for creating DbContext instances.</param>
    protected RepositoryBase(IDbFactory<TDbContext> dbFactory)
    {
        Context = dbFactory.GetDbContext() ?? throw new ArgumentNullException(nameof(dbFactory));
    }

    /// <inheritdoc />
    public async Task<int> CountAsync(ISpecification<TEntity> spec, CancellationToken cancellationToken = default)
    {
        return await ApplySpecification(spec).CountAsync(cancellationToken);
    }

    /// <inheritdoc />
    public abstract Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default);

    /// <inheritdoc />
    public abstract Task<TEntity?> GetBySpecAsync(ISpecification<TEntity> spec, CancellationToken cancellationToken = default);

    /// <inheritdoc />
    public abstract Task<IReadOnlyList<TEntity>> ListAllAsync(CancellationToken cancellationToken = default);

    /// <inheritdoc />
    public abstract Task<IReadOnlyList<TEntity>> ListAsync(ISpecification<TEntity> spec, CancellationToken cancellationToken = default);

    /// <summary>
    /// Applies the specified specification to the DbSet and returns the resulting IQueryable.
    /// </summary>
    /// <param name="spec">The specification to apply.</param>
    /// <returns>The IQueryable representing the result of applying the specification.</returns>
    protected IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> spec)
    {
        return SpecificationEvaluator<TEntity>.GetQuery(Context.Set<TEntity>().AsQueryable(), spec);
    }
}