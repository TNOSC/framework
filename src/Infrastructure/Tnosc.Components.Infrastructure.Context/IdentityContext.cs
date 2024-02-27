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

using System.Net.Http;
using System.Security.Claims;
using Tnosc.Components.Abstractions.Context;

namespace Tnosc.Components.Infrastructure.Context;

/// <summary>
/// Represents the context of an identity in the application.
/// </summary>
public class IdentityContext : IIdentityContext
{
    /// <summary>
    /// Gets a value indicating whether the identity is authenticated.
    /// </summary>
    public bool IsAuthenticated { get; }

    /// <summary>
    /// Gets the unique identifier of the identity.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Gets the roles associated with the identity.
    /// </summary>
    public IEnumerable<string> Roles { get; } = Enumerable.Empty<string>();

    /// <summary>
    /// Gets the claims associated with the identity.
    /// </summary>
    public Dictionary<string, IEnumerable<string>> Claims { get; } = [];

    // Private constructor for creating an empty identity context
    private IdentityContext()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="IdentityContext"/> class with a specified identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the identity.</param>
    public IdentityContext(Guid? id)
    {
        // Set properties based on the provided identifier
        Id = id ?? Guid.Empty;
        IsAuthenticated = id.HasValue;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="IdentityContext"/> class with a <see cref="ClaimsPrincipal"/>.
    /// </summary>
    /// <param name="principal">The claims principal representing the identity.</param>
    public IdentityContext(ClaimsPrincipal principal)
    {
        // Check if the principal and identity are valid
        if (principal is null || principal?.Identity is null || string.IsNullOrWhiteSpace(principal.Identity.Name))
        {
            return;
        }

        // Set properties based on the claims principal
        IsAuthenticated = principal.Identity?.IsAuthenticated is true;
        Id = IsAuthenticated ? Guid.Parse(principal.Identity!.Name) : Guid.Empty;

        // Extract roles and claims from the claims principal
        Roles = principal.Claims.Where(x => x.Type == ClaimTypes.Role)?.Select(e => e.Value) ?? Enumerable.Empty<string>();
        Claims = principal.Claims.GroupBy(x => x.Type)
            .ToDictionary(x => x.Key, x => x.Select(c => c.Value.ToString()));
    }

    /// <summary>
    /// Gets an empty instance of <see cref="IdentityContext"/>.
    /// </summary>
    public static IIdentityContext Empty => new IdentityContext();
}


