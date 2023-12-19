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

using System.Linq.Expressions;
using Tnosc.Components.Abstractions.Database.Common;

namespace Tnosc.Components.Abstractions.Database;
/// <summary>
/// Base implementation of the <see cref="ISpecification{T}"/> interface providing criteria,
/// includes, ordering, and paging functionality for querying entities.
/// </summary>
/// <typeparam name="T">The type of entity that the specification applies to.</typeparam>
public class SpecificationBase<T> : ISpecification<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SpecificationBase{T}"/> class.
    /// </summary>
    public SpecificationBase()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SpecificationBase{T}"/> class with the specified criteria.
    /// </summary>
    /// <param name="criteria">The criteria expression for filtering entities.</param>
    public SpecificationBase(Expression<Func<T, bool>> criteria)
    {
        Criteria = criteria;
    }

    /// <inheritdoc />
    public Expression<Func<T, bool>>? Criteria { get; }

    /// <inheritdoc />
    public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();

    /// <inheritdoc />
    public Expression<Func<T, object>>? OrderBy { get; private set; }

    /// <inheritdoc />
    public Expression<Func<T, object>>? OrderByDescending { get; private set; }

    /// <inheritdoc />
    public int Take { get; private set; }

    /// <inheritdoc />
    public int Skip { get; private set; }

    /// <inheritdoc />
    public bool IsPagingEnabled { get; private set; }

    /// <summary>
    /// Adds an include expression for related entities in the query result.
    /// </summary>
    /// <param name="includeExpression">The include expression to add.</param>
    protected void AddInclude(Expression<Func<T, object>> includeExpression)
    {
        Includes.Add(includeExpression);
    }

    /// <summary>
    /// Sets the ordering expression for ascending order in the query result.
    /// </summary>
    /// <param name="orderByExpression">The ordering expression to set.</param>
    protected void AddOrderBy(Expression<Func<T, object>> orderByExpression)
    {
        OrderBy = orderByExpression;
    }

    /// <summary>
    /// Sets the ordering expression for descending order in the query result.
    /// </summary>
    /// <param name="orderByDescExpression">The ordering expression to set for descending order.</param>
    protected void AddOrderByDescending(Expression<Func<T, object>> orderByDescExpression)
    {
        OrderByDescending = orderByDescExpression;
    }

    /// <summary>
    /// Applies paging to the query result.
    /// </summary>
    /// <param name="skip">The number of items to skip.</param>
    /// <param name="take">The number of items to take.</param>
    protected void ApplyPaging(int skip, int take)
    {
        Skip = skip;
        Take = take;
        IsPagingEnabled = true;
    }
}
