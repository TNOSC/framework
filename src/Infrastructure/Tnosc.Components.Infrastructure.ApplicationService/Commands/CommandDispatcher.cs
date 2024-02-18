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
using Microsoft.Extensions.DependencyInjection;

namespace Tnosc.Components.Infrastructure.ApplicationService.Commands;
/// <summary>
/// Implementation of the ICommandDispatcher interface responsible for dispatching commands
/// to their corresponding ICommandHandler for processing in a Command Query Responsibility Segregation (CQRS) system.
/// </summary>
/// <remarks>
/// Initializes a new instance of the CommandDispatcher class with the provided dependency injection container.
/// </remarks>
public sealed class CommandDispatcher : ICommandDispatcher
{
    private readonly IServiceProvider _serviceProvider;
    
    /// <summary>
    /// Initializes a new instance of the CommandDispatcher class with the provided dependency injection container.
    /// </summary>
    /// <param name="serviceProvider">The dependency injection container providing access to command handlers.</param>
    public CommandDispatcher(IServiceProvider serviceProvider)
        => _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    
    /// <summary>
    /// Dispatches the specified command to its corresponding ICommandHandler for processing.
    /// </summary>
    /// <typeparam name="TCommand">Type of the command to be dispatched.</typeparam>
    /// <param name="command">The command to be dispatched.</param>
    /// <param name="cancellationToken">Optional cancellation token for task cancellation.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    /// <remarks>
    /// If the provided command is null, the method returns early without invoking any handlers.
    /// The command handler is retrieved from the dependency injection container, and the command is passed for processing.
    /// </remarks>
    public async Task SendAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default)
        where TCommand : class, ICommand
    {
        // If the provided command is null, return early without processing.
        if (command is null)
            return;

        // Create a scoped service provider for resolving the command handler.
        using var scope = _serviceProvider.CreateScope();

        // Retrieve the required command handler for the specified command type.
        var handler = scope.ServiceProvider.GetRequiredService<ICommandHandler<TCommand>>();

        // Invoke the HandleAsync method on the command handler to process the command asynchronously.
        await handler.HandleAsync(command, cancellationToken);
    }
}