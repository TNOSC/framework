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

using Microsoft.EntityFrameworkCore;
using Tnosc.Components.Abstractions.Database.Common;

namespace Tnosc.Components.Abstractions.Database;
/// <summary>
/// Utility class for evaluating specifications and applying them to IQueryable entities.
/// </summary>
/// <typeparam name="TEntity">The type of entity that specifications apply to.</typeparam>
public class SpecificationEvaluator<TEntity> where TEntity : class
{
    /// <summary>
    /// Gets the IQueryable query with the specified specifications applied.
    /// </summary>
    /// <param name="inputQuery">The input IQueryable query to which specifications will be applied.</param>
    /// <param name="spec">The specification containing criteria, ordering, paging, and includes.</param>
    /// <returns>The IQueryable query with specifications applied.</returns>
    public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery,
        ISpecification<TEntity> spec)
    {
        var query = inputQuery;

        // Apply criteria if specified
        if (spec.Criteria != null)
        {
            query = query.Where(spec.Criteria);
        }

        // Apply ordering if specified
        if (spec.OrderBy != null)
        {
            query = query.OrderBy(spec.OrderBy);
        }

        // Apply descending ordering if specified
        if (spec.OrderByDescending != null)
        {
            query = query.OrderByDescending(spec.OrderByDescending);
        }

        // Apply paging if enabled
        if (spec.IsPagingEnabled)
        {
            query = query.Skip(spec.Skip).Take(spec.Take);
        }

        // Apply includes using Aggregate function
        query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));

        return query;
    }
}