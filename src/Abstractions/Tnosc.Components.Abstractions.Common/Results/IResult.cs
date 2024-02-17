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

namespace Tnosc.Components.Abstractions.Common.Results;
/// <summary>
/// Represents the result of an operation with a value.
/// </summary>
/// <typeparam name="TValue">The type of the result value.</typeparam>
public interface IResult<out TValue> : IResult
{
    /// <summary>
    /// Gets the value of the result.
    /// </summary>
    TValue Value { get; }
}
/// <summary>
/// Represents the result of an operation.
/// </summary>
public interface IResult
{
    /// <summary>
    /// Gets a value indicating whether the operation was successful.
    /// </summary>
    bool IsSuccess { get; }

    /// <summary>
    /// Gets a value indicating whether the operation was a failure.
    /// </summary>
    bool IsFailure { get; }

    /// <summary>
    /// Gets details about the error if the operation was a failure.
    /// </summary>
    Error Error { get; }
}