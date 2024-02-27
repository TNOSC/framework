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

namespace Tnosc.Components.Abstractions.Context;
/// <summary>
/// Represents the context of an identity in the application.
/// </summary>
public interface IIdentityContext
{
    /// <summary>
    /// Gets a value indicating whether the identity is authenticated.
    /// </summary>
    bool IsAuthenticated { get; }

    /// <summary>
    /// Gets the unique identifier of the identity.
    /// </summary>
    Guid Id { get; }

    /// <summary>
    /// Gets the roles associated with the identity.
    /// </summary>
    IEnumerable<string> Roles { get; }

    /// <summary>
    /// Gets the claims associated with the identity.
    /// </summary>
    Dictionary<string, IEnumerable<string>> Claims { get; }
}

