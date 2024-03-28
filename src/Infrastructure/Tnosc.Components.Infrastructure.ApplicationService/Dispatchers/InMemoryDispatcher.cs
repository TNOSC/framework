using Tnosc.Components.Abstractions.ApplicationService.Commands;
using Tnosc.Components.Abstractions.ApplicationService.Dispatchers;
using Tnosc.Components.Abstractions.ApplicationService.Events;
using Tnosc.Components.Abstractions.ApplicationService.Queries;
using Tnosc.Components.Abstractions.Common.Results;

namespace Tnosc.Components.Infrastructure.ApplicationService.Dispatchers;
/// <summary>
/// Implementation of the IDispatcher interface that utilizes in-memory dispatching of commands, events, and queries.
/// </summary>
public class InMemoryDispatcher : IDispatcher
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IEventDispatcher _eventDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    /// <summary>
    /// Initializes a new instance of the InMemoryDispatcher class with the specified command, event, and query dispatchers.
    /// </summary>
    /// <param name="commandDispatcher">The command dispatcher to be used for handling commands.</param>
    /// <param name="eventDispatcher">The event dispatcher to be used for handling events.</param>
    /// <param name="queryDispatcher">The query dispatcher to be used for handling queries.</param>
    public InMemoryDispatcher(ICommandDispatcher commandDispatcher, IEventDispatcher eventDispatcher,
        IQueryDispatcher queryDispatcher)
    {
        _commandDispatcher = commandDispatcher ?? throw new ArgumentNullException(nameof(commandDispatcher));
        _eventDispatcher = eventDispatcher ?? throw new ArgumentNullException(nameof(eventDispatcher));
        _queryDispatcher = queryDispatcher ?? throw new ArgumentNullException(nameof(queryDispatcher));
    }

    /// <inheritdoc/>
    public Task<TResult> SendAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default)
        where TResult : IResult
        => _commandDispatcher.SendAsync(command, cancellationToken);

    /// <inheritdoc/>
    public Task PublishAsync<T>(T @event, CancellationToken cancellationToken = default) where T : class, IEvent
        => _eventDispatcher.PublishAsync(@event, cancellationToken);

    /// <inheritdoc/>
    public Task<TResult> QueryAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
        where TResult : IResult
        => _queryDispatcher.QueryAsync(query, cancellationToken);
}