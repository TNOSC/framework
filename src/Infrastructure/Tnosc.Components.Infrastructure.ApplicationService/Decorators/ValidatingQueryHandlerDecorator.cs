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

using FluentValidation;
using Tnosc.Components.Abstractions.ApplicationService.Queries;
using Tnosc.Components.Abstractions.Common.Attributes;
using Tnosc.Components.Abstractions.Common.Results;
using Tnosc.Components.Infrastructure.Common.Results;

namespace Tnosc.Components.Infrastructure.ApplicationService.Decorators;
/// <summary>
/// Decorator for validating queries before handling them.
/// </summary>
/// <typeparam name="TQuery">The type of query to handle.</typeparam>
/// <typeparam name="TResult">The type of result returned by the query handler.</typeparam>
[Decorator]
public sealed class ValidatingQueryHandlerDecorator<TQuery, TResult> : IQueryHandler<TQuery, TResult>
    where TQuery : class, IQuery<TResult>
    where TResult : class, IResult
{
    private readonly IEnumerable<IValidator<TQuery>> _validators;
    private readonly IQueryHandler<TQuery, TResult> _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="ValidatingQueryHandlerDecorator{TQuery, TResult}"/> class.
    /// </summary>
    /// <param name="validators">The collection of validators for the query.</param>
    /// <param name="handler">The underlying query handler.</param>
    public ValidatingQueryHandlerDecorator(IEnumerable<IValidator<TQuery>> validators, IQueryHandler<TQuery, TResult> handler)
    {
        _validators = validators;
        _handler = handler;
    }

    /// <summary>
    /// Handles the specified query asynchronously after validation.
    /// </summary>
    /// <param name="query">The query to be handled.</param>
    /// <param name="cancellationToken">Optional cancellation token for task cancellation.</param>
    /// <returns>A task representing the asynchronous operation with the query result.</returns>
    public async Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken = default)
    {
        if (_validators.Any() is false)
        {
            return await _handler.HandleAsync(query, cancellationToken);
        }

        // Validate the query using the registered validators.
        Error[] errors = _validators
            .Select(validator => validator.Validate(query))
            .SelectMany(validationResult => validationResult.Errors)
            .Where(validationFailure => validationFailure is not null)
            .Select(failure => Error.Validation(failure.ErrorCode, failure.ErrorMessage))
            .Distinct()
            .ToArray();

        // If validation fails, return a validation result containing the errors.
        if (errors.Length is not 0)
        {
            return errors.CreateValidationResult<TResult>();
        }

        // Otherwise, handle the query using the underlying handler.
        return await _handler.HandleAsync(query, cancellationToken);
    }
}


