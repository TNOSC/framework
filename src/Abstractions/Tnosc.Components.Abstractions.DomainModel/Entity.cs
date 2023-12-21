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
/// Base abstract class for entities with a unique identifier of type TKey.
/// </summary>
/// <typeparam name="TKey">The type of the unique identifier.</typeparam>
public abstract class Entity<TKey> : IEntity<TKey>, IComparable<Entity<TKey>>, IComparable
    where TKey : struct
{
    private TKey _id;

    /// <summary>
    /// Default constructor for the entity. Initializes the identifier to the default value.
    /// </summary>
    protected Entity()
    {
        _id = default;
    }

    /// <summary>
    /// Constructor for the entity with a specified identifier.
    /// </summary>
    /// <param name="id">The identifier for the entity.</param>
    protected Entity(TKey id)
        : this()
    {
        _id = id;
    }

    /// <summary>
    /// Gets or sets the identifier for the entity.
    /// </summary>
    public virtual TKey Id
    {
        get { return _id; }
        protected internal set { _id = value; }
    }

    /// <summary>
    /// Inequality operator for comparing two entities.
    /// </summary>
    public static bool operator !=(Entity<TKey> left, Entity<TKey> right)
    {
        return !(left == right);
    }

    /// <summary>
    /// Equality operator for comparing two entities.
    /// </summary>
    public static bool operator ==(Entity<TKey> left, Entity<TKey> right)
    {
        if (left is null)
        {
            return right is null;
        }
        return left.Equals(right);
    }

    /// <summary>
    /// Compares the current entity to another object for ordering.
    /// </summary>
    public int CompareTo(object? obj)
    {
        return CompareTo(obj as Entity<TKey>);
    }

    /// <summary>
    /// Compares the current entity to another entity for ordering.
    /// </summary>
    public int CompareTo(Entity<TKey>? other)
    {
        return other is null ? 1 : ((IComparable)_id).CompareTo(other.Id);
    }

    /// <summary>
    /// Determines whether the current entity is equal to another entity.
    /// </summary>
    public virtual bool Equals(Entity<TKey> other)
    {
        if (other is null)
            return false;

        // Additional logic to handle default identifiers (assuming default is not a valid identifier)
        if (((IComparable)other.Id).CompareTo(default(TKey)) == 0 || ((IComparable)Id).CompareTo(default(TKey)) == 0)
            return false;

        return Equals(Id, other.Id);
    }

    /// <summary>
    /// Determines whether the current entity is equal to another object.
    /// </summary>
    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (GetType() != obj.GetType()) return false;
        var other = obj as Entity<TKey>;
        return other is not null && Equals(other);
    }

    /// <summary>
    /// Generates a hash code for the entity.
    /// </summary>
    public override int GetHashCode()
    {
        return (GetType().ToString() + Id).GetHashCode();
    }
}