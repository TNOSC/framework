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

namespace Tnosc.Components.Abstractions.DomainModel;
/// <summary>
/// Base abstract class for value objects.
/// </summary>
public abstract class ValueObject
{
    /// <summary>
    /// Checks equality between two value objects.
    /// </summary>
    protected static bool EqualOperator(ValueObject left, ValueObject right)
    {
        // Exclusive OR (^) is used to simplify the null-checking logic
        if (ReferenceEquals(left, null) ^ ReferenceEquals(right, null))
        {
            return false;
        }

        // If either side is null, the other side's Equals method will handle the comparison
        return ReferenceEquals(left, null) || left.Equals(right);
    }

    /// <summary>
    /// Checks inequality between two value objects.
    /// </summary>
    protected static bool NotEqualOperator(ValueObject left, ValueObject right)
    {
        return !(EqualOperator(left, right));
    }

    /// <summary>
    /// Gets the equality components of the value object.
    /// </summary>
    protected abstract IEnumerable<object> GetEqualityComponents();

    /// <summary>
    /// Overrides the Equals method to compare value objects for equality.
    /// </summary>
    public override bool Equals(object? obj)
    {
        // Check for null or different types
        if (obj is null || obj.GetType() != GetType())
        {
            return false;
        }

        // Cast to ValueObject and compare equality components
        var other = (ValueObject)obj;
        return this.GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    /// <summary>
    /// Overrides the GetHashCode method to generate a hash code for the value object.
    /// </summary>
    public override int GetHashCode()
    {
        // Combine hash codes of individual equality components using XOR (^)
        return GetEqualityComponents()
            .Select(x => x != null ? x.GetHashCode() : 0)
            .Aggregate((x, y) => x ^ y);
    }
}