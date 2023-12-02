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
using Tnosc.Components.Abstractrions.ApplicationService.Events;

namespace Tnosc.Components.Infrastructure.ApplicationService.Events;

/// <summary>
/// Implementation of the IEventDispatcher interface responsible for dispatching events
/// to their corresponding IEventHandler instances for processing.
/// </summary>
public sealed class EventDispatcher : IEventDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the EventDispatcher class with the provided dependency injection container.
    /// </summary>
    /// <param name="serviceProvider">The dependency injection container providing access to event handlers.</param>
    public EventDispatcher(IServiceProvider serviceProvider)
        => _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

    /// <summary>
    /// Publishes the specified event to its corresponding IEventHandler instances for processing.
    /// </summary>
    /// <typeparam name="TEvent">Type of the event to be published.</typeparam>
    /// <param name="event">The event to be published.</param>
    /// <param name="cancellationToken">Optional cancellation token for task cancellation.</param>
    /// <returns>A Task representing the asynchronous operation with the event publication.</returns>
    /// <remarks>
    /// The method creates a scope, retrieves all IEventHandler instances for the specified event type,
    /// and invokes their HandleAsync methods concurrently using Task.WhenAll.
    /// </remarks>
    public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
        where TEvent : class, IEvent
    {
        // Create a scoped service provider for resolving the event handler.
        using var scope = _serviceProvider.CreateScope();

        // Retrieve all event handlers for the specified event type.
        var handlers = scope.ServiceProvider.GetServices<IEventHandler<TEvent>>();

        // Invoke HandleAsync on each event handler concurrently using Task.WhenAll.
        var tasks = handlers.Select(handler => handler.HandleAsync(@event, cancellationToken));
        await Task.WhenAll(tasks);
    }
}