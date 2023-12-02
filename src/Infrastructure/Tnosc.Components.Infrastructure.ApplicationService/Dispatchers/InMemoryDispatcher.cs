using Tnosc.Components.Abstractrions.ApplicationService.Commands;
using Tnosc.Components.Abstractrions.ApplicationService.Dispatchers;
using Tnosc.Components.Abstractrions.ApplicationService.Events;
using Tnosc.Components.Abstractrions.ApplicationService.Queries;

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

    /// <summary>
    /// Dispatches the specified command for in-memory processing.
    /// </summary>
    /// <typeparam name="T">Type of the command to be dispatched.</typeparam>
    /// <param name="command">The command to be dispatched.</param>
    /// <param name="cancellationToken">Optional cancellation token for task cancellation.</param>
    /// <returns>A Task representing the asynchronous operation of command dispatching.</returns>
    public Task SendAsync<T>(T command, CancellationToken cancellationToken = default) where T : class, ICommand
        => _commandDispatcher.SendAsync(command, cancellationToken);

    /// <summary>
    /// Publishes the specified event for in-memory processing.
    /// </summary>
    /// <typeparam name="T">Type of the event to be published.</typeparam>
    /// <param name="event">The event to be published.</param>
    /// <param name="cancellationToken">Optional cancellation token for task cancellation.</param>
    /// <returns>A Task representing the asynchronous operation of event publishing.</returns>
    public Task PublishAsync<T>(T @event, CancellationToken cancellationToken = default) where T : class, IEvent
        => _eventDispatcher.PublishAsync(@event, cancellationToken);

    /// <summary>
    /// Executes the specified query for in-memory processing.
    /// </summary>
    /// <typeparam name="TResult">Type of the result returned by the query.</typeparam>
    /// <param name="query">The query to be executed.</param>
    /// <param name="cancellationToken">Optional cancellation token for task cancellation.</param>
    /// <returns>A Task representing the asynchronous operation of query execution with the query result.</returns>
    public Task<TResult> QueryAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
        => _queryDispatcher.QueryAsync(query, cancellationToken);
}