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

using Tnosc.Components.Abstractions.DomainModel.Common;

namespace Tnosc.Components.Abstractions.DomainModel;
/// <summary>
/// Base abstract class for auditable entities with a unique identifier of type TKey.
/// </summary>
/// <typeparam name="TKey">The type of the unique identifier.</typeparam>
public abstract class AuditableEntity<TKey> : Entity<TKey>, IAuditableEntity<TKey>
    where TKey : struct
{
    /// <summary>
    /// Gets or sets the username or identifier of the user who created the entity.
    /// </summary>
    public string? CreatedBy { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the entity was created.
    /// </summary>
    public DateTime? CreatedOn { get; set; }

    /// <summary>
    /// Gets or sets the username or identifier of the user who last updated the entity.
    /// </summary>
    public string? UpdatedBy { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the entity was last updated.
    /// </summary>
    public DateTime? UpdatedOn { get; set; }
}

