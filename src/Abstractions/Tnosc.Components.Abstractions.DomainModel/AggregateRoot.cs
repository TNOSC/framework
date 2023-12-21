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

using System.Collections.ObjectModel;
using Tnosc.Components.Abstractions.DomainModel.Common;

namespace Tnosc.Components.Abstractions.DomainModel;
/// <summary>
/// Base abstract class for aggregate roots with a unique identifier of type TKey.
/// </summary>
/// <typeparam name="TKey">The type of the unique identifier.</typeparam>
public abstract class AggregateRoot<TKey> : AuditableEntity<TKey>, IAggregateRoot<TKey>
    where TKey : struct
{
    private readonly Collection<IDomainEvent> _domainEvents = new();

    /// <summary>
    /// Gets the read-only list of domain events associated with the aggregate root.
    /// </summary>
    public virtual IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents;

    /// <summary>
    /// Gets or sets the version of the aggregate root.
    /// </summary>
    public int Version { get; private set; }

    /// <summary>
    /// Clears the list of domain events associated with the aggregate root.
    /// </summary>
    public virtual void ClearEvents()
    {
        _domainEvents.Clear();
    }

    /// <summary>
    /// Adds a domain event to the list associated with the aggregate root.
    /// </summary>
    /// <param name="event">The domain event to be added.</param>
    protected virtual void AddDomainEvent(IDomainEvent @event)
    {
        _domainEvents.Add(@event);
    }
}