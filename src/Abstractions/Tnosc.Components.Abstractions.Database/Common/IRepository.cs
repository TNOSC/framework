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

namespace Tnosc.Components.Abstractions.Database.Common;
/// <summary>
/// Represents a generic repository interface for querying on entities.
/// </summary>
/// <typeparam name="TEntity">The type of entity handled by the repository.</typeparam>
/// <typeparam name="TKey">The type of the entity's primary key.</typeparam>
public interface IRepository<TEntity, TKey>
    where TEntity : class
{
    /// <summary>
    /// Asynchronously retrieves an entity by its primary key.
    /// </summary>
    /// <param name="id">The primary key of the entity to retrieve.</param>
    /// <param name="cancellationToken">The cancellation token for the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, returning the entity with the specified primary key.</returns>
    Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves an entity based on a specification.
    /// </summary>
    /// <param name="spec">The specification defining the criteria for entity retrieval.</param>
    /// <param name="cancellationToken">The cancellation token for the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, returning the entity that satisfies the specified criteria.</returns>
    Task<TEntity?> GetBySpecAsync(ISpecification<TEntity> spec, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves all entities in the repository.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token for the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, returning a list of all entities.</returns>
    Task<IReadOnlyList<TEntity>> ListAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves entities based on a specification.
    /// </summary>
    /// <param name="spec">The specification defining the criteria for entity retrieval.</param>
    /// <param name="cancellationToken">The cancellation token for the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, returning a list of entities that satisfy the specified criteria.</returns>
    Task<IReadOnlyList<TEntity>> ListAsync(ISpecification<TEntity> spec, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously counts the number of entities based on a specification.
    /// </summary>
    /// <param name="spec">The specification defining the criteria for counting entities.</param>
    /// <param name="cancellationToken">The cancellation token for the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, returning the count of entities that satisfy the specified criteria.</returns>
    Task<int> CountAsync(ISpecification<TEntity> spec, CancellationToken cancellationToken = default);
}
