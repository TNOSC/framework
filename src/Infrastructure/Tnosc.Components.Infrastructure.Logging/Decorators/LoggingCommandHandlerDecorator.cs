/*
 Copyright (c) 2024 Ahmed HEDFI (ahmed.hedfi@gmail.com)

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

using Humanizer;
using Microsoft.Extensions.Logging;
using Tnosc.Components.Abstractions.ApplicationService.Commands;
using Tnosc.Components.Abstractions.Common.Attributes;
using Tnosc.Components.Abstractions.Common.Results;
using Tnosc.Components.Abstractions.Context;
using Tnosc.Components.Infrastructure.Common;

namespace Tnosc.Components.Infrastructure.Logging.Decorators;
/// <summary>
/// Decorator for logging command handling operations.
/// </summary>
[Decorator]
public sealed class LoggingCommandHandlerDecorator<TCommand, TResult> : ICommandHandler<TCommand, TResult>
    where TCommand : class, ICommand<TResult>
    where TResult : class, IResult
{
    private readonly ICommandHandler<TCommand, TResult> _handler;
    private readonly IContext _context;
    private readonly ILogger<LoggingCommandHandlerDecorator<TCommand, TResult>> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="LoggingCommandHandlerDecorator{T}"/> class.
    /// </summary>
    /// <param name="handler">The decorated command handler.</param>
    /// <param name="context">The context of the current operation.</param>
    /// <param name="logger">The logger for logging command handling operations.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="logger"/> is null.</exception>
    public LoggingCommandHandlerDecorator(
        ICommandHandler<TCommand, TResult> handler,
        IContext context,
        ILogger<LoggingCommandHandlerDecorator<TCommand, TResult>> logger)
    {
        _handler = handler;
        _context = context;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Handles the specified command asynchronously.
    /// </summary>
    /// <param name="command">The command to handle.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken = default)
    {
        var module = command.GetModuleName();
        var name = command.GetType().Name.Underscore();
        var requestId = _context.RequestId;
        var traceId = _context.TraceId;
        var userId = _context.Identity?.Id;
        var correlationId = _context.CorrelationId;
        _logger.LogInformation("Handling a command: {Name} ({Module}) [Request ID: {RequestId}, Correlation ID: {CorrelationId}, Trace ID: '{TraceId}', User ID: '{UserId}]'...",
            name, module, requestId, correlationId, traceId, userId);
       var result = await _handler.HandleAsync(command, cancellationToken);
        _logger.LogInformation("Handled a command: {Name} ({Module}) [Request ID: {RequestId}, Correlation ID: {CorrelationId}, Trace ID: '{TraceId}', User ID: '{UserId}']",
            name, module, requestId, correlationId, traceId, userId);
        return result;
    }
}

