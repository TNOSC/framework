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

using Tnosc.Components.Abstractions.Common.Results;

namespace Tnosc.Components.Infrastructure.Common.Results;
/// <summary>
/// Represents the result of a validation operation with possible validation errors.
/// </summary>
/// <typeparam name="TValue">The result value type.</typeparam>
public sealed class ValidationResult<TValue> : Result<TValue>, IValidationResult
{
    // Private constructor for a failure result with validation errors
    private ValidationResult(Error[] validationErrors)
        : base(default, Error.ValidationError)
    {
        ValidationErrors = validationErrors;
    }

    // Private constructor for a success result without validation errors
    private ValidationResult(TValue? value)
        : base(value, Error.None)
    {
        ValidationErrors = Array.Empty<Error>();
    }

    /// <summary>
    /// Gets an array of validation errors.
    /// </summary>
    public Error[] ValidationErrors { get; }

    /// <summary>
    /// Creates a failure ValidationResult&lt;TValue&gt; with the specified validation errors.
    /// </summary>
    /// <param name="validationErrors">Validation errors.</param>
    /// <returns>A failure ValidationResult&lt;TValue&gt;.</returns>
    public static ValidationResult<TValue> WithErrors(Error[] validationErrors)
    {
        return new(validationErrors);
    }

    /// <summary>
    /// Creates a success ValidationResult&lt;TValue&gt; without validation errors.
    /// </summary>
    /// <param name="value">Result value.</param>
    /// <returns>A success ValidationResult&lt;TValue&gt;.</returns>
    public static ValidationResult<TValue> WithoutErrors(TValue? value)
    {
        return new(value);
    }
}
/// <summary>
/// Represents the result of a validation operation without a specific result value.
/// </summary>
public sealed class ValidationResult : Result, IValidationResult
{
    // Predefined success instance for validation results
    private static readonly ValidationResult _successValidationResult = new();

    // Private constructor for a failure result with validation errors
    private ValidationResult(Error[] validationErrors)
        : base(Error.ValidationError)
    {
        ValidationErrors = validationErrors;
    }

    // Private constructor for a success result without validation errors
    private ValidationResult()
        : base(Error.None)
    {
        ValidationErrors = Array.Empty<Error>();
    }

    /// <summary>
    /// Gets an array of validation errors.
    /// </summary>
    public Error[] ValidationErrors { get; }

    /// <summary>
    /// Creates a failure ValidationResult with the specified validation errors.
    /// </summary>
    /// <param name="validationErrors">Validation errors.</param>
    /// <returns>A failure ValidationResult.</returns>
    public static ValidationResult WithErrors(ICollection<Error> validationErrors)
    {
        return new(validationErrors.ToArray());
    }

    /// <summary>
    /// Creates a success ValidationResult without validation errors.
    /// </summary>
    /// <returns>A success ValidationResult.</returns>
    public static ValidationResult WithoutErrors()
    {
        return _successValidationResult;
    }
}