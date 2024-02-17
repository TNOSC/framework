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
/// Represents an error object with a code, description, and type.
/// </summary>
public sealed record Error
{
    /// <summary>
    /// Gets or sets the error code.
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Gets or sets the error description.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the type of error.
    /// </summary>
    public ErrorType Type { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Error"/> class.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="description">The error description.</param>
    /// <param name="errorType">The type of error.</param>
    private Error(string code, string description, ErrorType errorType)
    {
        Code = code;
        Description = description;
        Type = errorType;
    }

    /// <summary>
    /// Throws an exception if the current error is the predefined 'None' instance.
    /// </summary>
    public void ThrowIfErrorNone()
    {
        if (this == None)
        {
            throw new InvalidOperationException("Provided error is Error.None");
        }
    }

    /// <summary>
    /// Represents a predefined error indicating success or no error.
    /// </summary>
    public static readonly Error None = new(string.Empty, string.Empty, ErrorType.Failure);

    /// <summary>
    /// Represents a predefined error indicating a null value was provided.
    /// </summary>
    public static readonly Error NullValue = new("Error.NullValue", "Null value was provided", ErrorType.Failure);

    /// <summary>
    /// The condition not met error instance.
    /// </summary>
    public static readonly Error ConditionNotSatisfied = new("Error.ConditionNotSatisfied", "The specified condition was not satisfied.", ErrorType.Failure);

    /// <summary>
    /// The validation error instance
    /// </summary>
    public static readonly Error ValidationError = new($"{nameof(ValidationError)}", "A validation problem occurred.", ErrorType.Validation);

    /// <summary>
    /// Creates a new error of type 'NotFound'.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="description">The error description.</param>
    /// <returns>A new instance of the 'Error' class representing a 'NotFound' error.</returns>
    public static Error NotFound(string code, string description) =>
        new(code, description, ErrorType.NotFound);

    /// <summary>
    /// Creates a new error of type 'Validation'.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="description">The error description.</param>
    /// <returns>A new instance of the 'Error' class representing a 'Validation' error.</returns>
    public static Error Validation(string code, string description) =>
        new(code, description, ErrorType.Validation);

    /// <summary>
    /// Creates a new error of type 'Failure'.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="description">The error description.</param>
    /// <returns>A new instance of the 'Error' class representing a 'Failure' error.</returns>
    public static Error Failure(string code, string description) =>
        new(code, description, ErrorType.Failure);

    /// <summary>
    /// Creates a new error of type 'Conflict'.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="description">The error description.</param>
    /// <returns>A new instance of the 'Error' class representing a 'Conflict' error.</returns>
    public static Error Conflict(string code, string description) =>
        new(code, description, ErrorType.Conflict);

    /// <summary>
    /// Creates an error instance of type 'Unauthorized' with the specified code and description.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="description">The error description.</param>
    /// <returns>An error instance representing an 'Unauthorized' error.</returns>
    public static Error Unauthorized(string code, string description) =>
        new(code, description, ErrorType.Unauthorized);

    /// <summary>
    /// Creates an error instance of type 'Forbidden' with the specified code and description.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="description">The error description.</param>
    /// <returns>An error instance representing a 'Forbidden' error.</returns>
    public static Error Forbidden(string code, string description) =>
        new(code, description, ErrorType.Forbidden);

}