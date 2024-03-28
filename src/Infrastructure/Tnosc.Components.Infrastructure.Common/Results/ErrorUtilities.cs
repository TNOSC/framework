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

public static class ErrorUtilities
{
    public static TResult CreateValidationResult<TResult>(this ICollection<Error> errors)
        where TResult : class, IResult
    {
        if (typeof(TResult) == typeof(Result))
        {
            return (ValidationResult.WithErrors(errors) as TResult)!;
        }

        object validationResult = typeof(ValidationResult<>)
            .GetGenericTypeDefinition()
            .MakeGenericType(typeof(TResult).GenericTypeArguments[0])
            .GetMethod(nameof(ValidationResult.WithErrors))!
            .Invoke(null, [errors])!;

        return (TResult)validationResult;
    }

    /// <summary>
    /// Add error to the list if the condition is true
    /// </summary>
    /// <param name="errors">Error list</param>
    /// <param name="condition">Validation condition</param>
    /// <param name="error">Error to add to list if the condition is true</param>
    /// <returns>Same instance of error list</returns>
    public static IList<Error> If
    (
        this IList<Error> errors,
        bool condition,
        Error error
    )
    {
        if (condition is true)
        {
            errors.Add(error);
        }

        return errors;
    }

    /// <summary>
    /// Use external validation that usually group multiple validations into logic group
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="errors">Error list</param>
    /// <param name="validationSegment">Func that group validations into logic group</param>
    /// <param name="valueUnderValidation">Value that is being validated</param>
    /// <returns>Same instance of error list</returns>
    public static IList<Error> UseValidation<TValue>
    (
        this IList<Error> errors,
        Func<IList<Error>, TValue, IList<Error>> validationSegment,
        TValue valueUnderValidation
    )
    {
        return validationSegment(errors, valueUnderValidation);
    }

    /// <summary>
    /// Use external validation that usually group multiple validations into logic group
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="errors">Error list</param>
    /// <param name="validationSegment">Func that group validations into logic group</param>
    /// <param name="valueUnderValidation">Value that is being validated</param>
    /// <returns>Same instance of error list</returns>
    public static IList<Error> UseValidation<TValue>
    (
        this IList<Error> errors,
        Func<TValue, IList<Error>> validationSegment,
        TValue valueUnderValidation
    )
    {
        var errorsToAdd = validationSegment(valueUnderValidation);

        foreach (var errorToAdd in errorsToAdd)
        {
            errorsToAdd.Add(errorToAdd);
        }

        return errors;
    }
}