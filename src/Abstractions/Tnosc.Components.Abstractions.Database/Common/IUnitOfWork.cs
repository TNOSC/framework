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

namespace Tnosc.Components.Abstractions.Database.Common;
/// <summary>
/// Represents a unit of work for managing transactions and committing changes in a specific DbContext.
/// </summary>
/// <typeparam name="TDbContext">The type of the DbContext associated with the unit of work.</typeparam>
public interface IUnitOfWork<TDbContext> : IDisposable
    where TDbContext : DbContext
{
    /// <summary>
    /// Asynchronously commits changes to the underlying database.
    /// </summary>
    /// <returns>A task representing the asynchronous completion of the commit operation, returning the number of affected rows.</returns>
    Task<int> CompleteAsync();
}