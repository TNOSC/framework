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

using Tnosc.Components.Abstractions.Common.Results;

namespace Tnosc.Components.Abstractions.ApplicationService.Commands;

public interface ICommandHandler<in TCommand, TResult> 
    where TCommand : class, ICommand<TResult>
    where TResult : IResult
{
    /// <summary>
    /// Handles the specified command asynchronously and returns the result.
    /// </summary>
    /// <param name="command">The command to be handled.</param>
    /// <param name="cancellationToken">Optional cancellation token for task cancellation.</param>
    /// <returns>A task representing the asynchronous operation with the command result.</returns>
    Task<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken = default);

}