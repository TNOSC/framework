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
using Tnosc.Components.Abstractions.ApplicationService.Events;
using Tnosc.Components.Abstractions.Common.Attributes;
using Tnosc.Components.Abstractions.Context;
using Tnosc.Components.Infrastructure.Common;

namespace Tnosc.Components.Infrastructure.Logging.Decorators;
/// <summary>
/// Decorator for logging event handling operations.
/// </summary>
[Decorator]
public sealed class LoggingEventHandlerDecorator<T> : IEventHandler<T> where T : class, IEvent
{
    private readonly IEventHandler<T> _handler;
    private readonly IContext _context;
    private readonly ILogger<LoggingEventHandlerDecorator<T>> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="LoggingEventHandlerDecorator{T}"/> class.
    /// </summary>
    /// <param name="handler">The decorated event handler.</param>
    /// <param name="context">The context of the current operation.</param>
    /// <param name="logger">The logger for logging event handling operations.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="logger"/> is null.</exception>
    public LoggingEventHandlerDecorator(
        IEventHandler<T> handler,
        IContext context,
        ILogger<LoggingEventHandlerDecorator<T>> logger)
    {
        _handler = handler;
        _context = context;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Handles the specified event asynchronously.
    /// </summary>
    /// <param name="event">The event to handle.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task HandleAsync(T @event, CancellationToken cancellationToken = default)
    {
        var module = @event.GetModuleName();
        var name = @event.GetType().Name.Underscore();
        var requestId = _context.RequestId;
        var traceId = _context.TraceId;
        var userId = _context.Identity?.Id;
        var correlationId = _context.CorrelationId;
        _logger.LogInformation("Handling an event: {Name} ({Module}) [Request ID: {RequestId}, Correlation ID: {CorrelationId}, Trace ID: '{TraceId}', User ID: '{UserId}]...",
            name, module, requestId, correlationId, traceId, userId);
        await _handler.HandleAsync(@event, cancellationToken);
        _logger.LogInformation("Handled an event: {Name} ({Module}) [Request ID: {RequestId}, Correlation ID: {CorrelationId}, Trace ID: '{TraceId}', User ID: '{UserId}]",
            name, module, requestId, correlationId, traceId, userId);
    }
}
