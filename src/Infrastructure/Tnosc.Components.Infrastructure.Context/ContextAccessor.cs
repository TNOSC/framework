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

using Tnosc.Components.Abstractions.Context;

namespace Tnosc.Components.Infrastructure.Context;

/// <summary>
/// Provides access to the context information within a specific execution context.
/// </summary>
public sealed class ContextAccessor
{
    // AsyncLocal is used to store the contextual information for each asynchronous execution context
    private static readonly AsyncLocal<ContextHolder> Holder = new();

    /// <summary>
    /// Gets or sets the context information associated with the current execution context.
    /// </summary>
    public IContext Context
    {
        get => Holder.Value!.Context!;
        set
        {
            // Retrieve the current context holder
            var holder = Holder.Value;

            // Clear the existing context if present
            if (holder != null)
            {
                holder.Context = null;
            }

            // Set the new context if provided
            if (value != null)
            {
                Holder.Value = new ContextHolder { Context = value };
            }
        }
    }

    // Private class to hold the context information within the AsyncLocal
    private class ContextHolder
    {
        public IContext? Context;
    }
}


