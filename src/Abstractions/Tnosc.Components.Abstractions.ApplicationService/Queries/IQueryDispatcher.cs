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

using Tnosc.Components.Abstractions.Common.Results;

namespace Tnosc.Components.Abstractions.ApplicationService.Queries;
/// <summary>
/// Represents an interface for a query dispatcher responsible for executing queries asynchronously.
/// </summary>
public interface IQueryDispatcher
{
    /// <summary>
    /// Executes the specified query asynchronously and returns the result.
    /// </summary>
    /// <typeparam name="TResult">The type of result produced by the query.</typeparam>
    /// <param name="query">The query to execute.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains the query result.</returns>
    Task<TResult> QueryAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
        where TResult : IResult;

}