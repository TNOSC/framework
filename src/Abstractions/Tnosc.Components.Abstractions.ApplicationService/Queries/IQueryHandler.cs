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

namespace Tnosc.Components.Abstractions.ApplicationService.Queries;
/// <summary>
/// Represents an interface for a query handler responsible for handling a specific type of query and returning a result asynchronously.
/// </summary>
/// <typeparam name="TQuery">Type of the query to be handled.</typeparam>
/// <typeparam name="TResult">Type of the result returned by the query.</typeparam>
public interface IQueryHandler<in TQuery, TResult> where TQuery : class, IQuery<TResult>
{
    /// <summary>
    /// Handles the specified query asynchronously and returns the result.
    /// </summary>
    /// <param name="query">The query to be handled.</param>
    /// <param name="cancellationToken">Optional cancellation token for task cancellation.</param>
    /// <returns>A task representing the asynchronous operation with the query result.</returns>
    Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken = default);
}