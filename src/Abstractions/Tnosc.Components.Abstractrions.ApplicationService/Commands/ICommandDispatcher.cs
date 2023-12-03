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

namespace Tnosc.Components.Abstractrions.ApplicationService.Commands;
/// <summary>
/// Represents an interface for a command dispatcher responsible for sending commands asynchronously.
/// </summary>
public interface ICommandDispatcher
{
    /// <summary>
    /// Sends a specified command asynchronously.
    /// </summary>
    /// <typeparam name="TCommand">Type of the command to be sent.</typeparam>
    /// <param name="command">The command to be sent.</param>
    /// <param name="cancellationToken">Optional cancellation token for task cancellation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SendAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default) 
            where TCommand : class, ICommand;
}