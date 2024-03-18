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
using Tnosc.Components.Abstractions.ApplicationService.Queries;
using Tnosc.Components.Abstractions.Context;
using Tnosc.Components.Infrastructure.Common;
using Tnosc.Components.Infrastructure.Common.Attributes;

namespace Tnosc.Components.Infrastructure.Logging.Decorators;
/// <summary>
/// Decorator for logging query handling operations.
/// </summary>
[Decorator]
public sealed class LoggingQueryHandlerDecorator<TQuery, TResult> : IQueryHandler<TQuery, TResult>
    where TQuery : class, IQuery<TResult>
{
    private readonly IQueryHandler<TQuery, TResult> _handler;
    private readonly IContext _context;
    private readonly ILogger<LoggingQueryHandlerDecorator<TQuery, TResult>> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="LoggingQueryHandlerDecorator{TQuery, TResult}"/> class.
    /// </summary>
    /// <param name="handler">The decorated query handler.</param>
    /// <param name="context">The context of the current operation.</param>
    /// <param name="logger">The logger for logging query handling operations.</param>
    public LoggingQueryHandlerDecorator(IQueryHandler<TQuery, TResult> handler,
        IContext context, ILogger<LoggingQueryHandlerDecorator<TQuery, TResult>> logger)
    {
        _handler = handler;
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Handles the specified query asynchronously.
    /// </summary>
    /// <param name="query">The query to handle.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken = default)
    {
        var module = query.GetModuleName();
        var name = query.GetType().Name.Underscore();
        var requestId = _context.RequestId;
        var correlationId = _context.CorrelationId;
        var traceId = _context.TraceId;
        var userId = _context.Identity?.Id;
        _logger.LogInformation("Handling a query: {Name} ({Module}) [Request ID: {RequestId}, Correlation ID: {CorrelationId}, Trace ID: '{TraceId}', User ID: '{UserId}]...",
            name, module, requestId, correlationId, traceId, userId);
        var result = await _handler.HandleAsync(query, cancellationToken);
        _logger.LogInformation("Handled a query: {Name} ({Module}) [Request ID: {RequestId}, Correlation ID: {CorrelationId}, Trace ID: '{TraceId}', User ID: '{UserId}]",
            name, module, requestId, correlationId, traceId, userId);

        return result;
    }
}

