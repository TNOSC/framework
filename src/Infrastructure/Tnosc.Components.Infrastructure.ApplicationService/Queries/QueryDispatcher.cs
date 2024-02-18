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

using Microsoft.Extensions.DependencyInjection;
using Tnosc.Components.Abstractions.ApplicationService.Queries;

namespace Tnosc.Components.Infrastructure.ApplicationService.Queries;
/// <summary>
/// Implementation of the IQueryDispatcher interface responsible for dispatching queries
/// to their corresponding IQueryHandler for processing in a Command Query Responsibility Segregation (CQRS) system.
/// </summary>
public sealed class QueryDispatcher : IQueryDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the QueryDispatcher class with the provided dependency injection container.
    /// </summary>
    /// <param name="serviceProvider">The dependency injection container providing access to query handlers.</param>
    public QueryDispatcher(IServiceProvider serviceProvider)
        => _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

    /// <summary>
    /// Dispatches the specified query to its corresponding IQueryHandler for processing.
    /// </summary>
    /// <typeparam name="TResult">Type of the result returned by the query.</typeparam>
    /// <param name="query">The query to be dispatched.</param>
    /// <param name="cancellationToken">Optional cancellation token for task cancellation.</param>
    /// <returns>A Task representing the asynchronous operation with the query result.</returns>
    /// <remarks>
    /// The method uses reflection to dynamically obtain the corresponding IQueryHandler and invoke its HandleAsync method.
    /// If the query handler or its HandleAsync method is not found, an InvalidOperationException is thrown.
    /// </remarks>
    public async Task<TResult> QueryAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
    {
        // Create a scoped service provider for resolving the query handler.
        using var scope = _serviceProvider.CreateScope();

        // Dynamically create the type of the query handler based on the query type and result type.
        var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));

        // Retrieve the query handler instance from the dependency injection container.
        var handler = scope.ServiceProvider.GetRequiredService(handlerType);

        // Get the MethodInfo for the HandleAsync method of the IQueryHandler interface.
        var method = handlerType.GetMethod(nameof(IQueryHandler<IQuery<TResult>, TResult>.HandleAsync));

        // If the HandleAsync method is not found, throw an InvalidOperationException.
        if (method is null)
            throw new InvalidOperationException($"Query handler for '{typeof(TResult).Name}' is invalid.");

#pragma warning disable CS8600,CS8602 // disables warnings about converting a null literal or a possible null value to a non-nullable type or dereferencing a possibly null reference.
        // Invoke the HandleAsync method on the query handler and await the result.
        return await (Task<TResult>)method.Invoke(handler, new object[] { query, cancellationToken });
#pragma warning restore CS8602,CS8600
    }
}