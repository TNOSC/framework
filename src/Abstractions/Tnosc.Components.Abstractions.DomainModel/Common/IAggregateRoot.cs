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

namespace Tnosc.Components.Abstractions.DomainModel.Common;
/// <summary>
/// Represents an aggregate root in the domain, combining auditable and versioned entity features.
/// </summary>
/// <typeparam name="TKey">The type of the unique identifier.</typeparam>
public interface IAggregateRoot<TKey> : IAuditableEntity<TKey>
{
    /// <summary>
    /// Gets the version of the aggregate root.
    /// </summary>
    int Version { get; }

    /// <summary>
    /// Gets the read-only list of domain events associated with the aggregate root.
    /// </summary>
    IReadOnlyList<IDomainEvent> DomainEvents { get; }

    /// <summary>
    /// Clears the list of domain events associated with the aggregate root.
    /// </summary>
    void ClearEvents();
}