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
/// <summary>
/// Represents an interface for a command dispatcher responsible for sending commands asynchronously.
/// </summary>
public interface ICommandDispatcher
{
    /// <summary>
    /// Sends a command asynchronously and returns a result of type <typeparamref name="TResult"/>.
    /// </summary>
    /// <typeparam name="TResult">The type of result produced by the command.</typeparam>
    /// <param name="command">The command to send.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation, containing the result of type <typeparamref name="TResult"/>.</returns>
    Task<TResult> SendAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default)
        where TResult : IResult;



}