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
 
using Tnosc.Components.Abstractions.ApplicationService.Commands;
using Tnosc.Components.Abstractions.ApplicationService.Events;
using Tnosc.Components.Abstractions.ApplicationService.Queries;
using Tnosc.Components.Abstractions.Common.Results;

namespace Tnosc.Components.Abstractions.ApplicationService.Dispatchers;
/// <summary>
/// Represents a general-purpose dispatcher interface for sending commands, publishing events, and executing queries asynchronously.
/// </summary>
public interface IDispatcher
{
    /// <summary>
    /// Sends a command asynchronously and returns a result of type <typeparamref name="TResult"/>.
    /// </summary>
    /// <typeparam name="TResult">The type of result produced by executing the command.</typeparam>
    /// <param name="command">The command to send.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation, containing the result of type <typeparamref name="TResult"/>.</returns>
    Task<TResult> SendAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default)
        where TResult : IResult;

    /// <summary>
    /// Publishes a specified event asynchronously.
    /// </summary>
    /// <typeparam name="T">Type of the event to be published.</typeparam>
    /// <param name="event">The event to be published.</param>
    /// <param name="cancellationToken">Optional cancellation token for task cancellation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task PublishAsync<T>(T @event, CancellationToken cancellationToken = default) 
        where T : class, IEvent;

    /// <summary>
    /// Executes a query asynchronously and returns a result of type <typeparamref name="TResult"/>.
    /// </summary>
    /// <typeparam name="TResult">The type of result produced by executing the query.</typeparam>
    /// <param name="query">The query to execute.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation, containing the result of type <typeparamref name="TResult"/>.</returns>
    Task<TResult> QueryAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
        where TResult : IResult;

}