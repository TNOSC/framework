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

using Microsoft.Extensions.Configuration;

namespace Tnosc.Components.Infrastructure.Common;
/// <summary>
/// Provides extension methods for IConfiguration and related classes.
/// </summary>
public static class Extensisons
{
    /// <summary>
    /// Retrieves options from the specified configuration section and binds them to a new instance of the specified type.
    /// </summary>
    /// <typeparam name="T">The type of options to retrieve.</typeparam>
    /// <param name="configuration">The configuration to retrieve options from.</param>
    /// <param name="sectionName">The name of the configuration section containing the options.</param>
    /// <returns>An instance of the specified options type populated with values from the configuration.</returns>
    public static T GetOptions<T>(this IConfiguration configuration, string sectionName) where T : new()
        => configuration.GetSection(sectionName).GetOptions<T>();

    /// <summary>
    /// Retrieves options from the specified configuration section and binds them to a new instance of the specified type.
    /// </summary>
    /// <typeparam name="T">The type of options to retrieve.</typeparam>
    /// <param name="section">The configuration section containing the options.</param>
    /// <returns>An instance of the specified options type populated with values from the configuration section.</returns>
    public static T GetOptions<T>(this IConfigurationSection section) where T : new()
    {
        var options = new T();
        section.Bind(options);
        return options;
    }

    /// <summary>
    /// Gets the module name associated with the specified object.
    /// </summary>
    /// <param name="value">The object to get the module name from.</param>
    /// <returns>The module name if available; otherwise, an empty string.</returns>
    public static string GetModuleName(this object value)
        => value?.GetType().GetModuleName() ?? string.Empty;

    /// <summary>
    /// Gets the module name associated with the specified type.
    /// </summary>
    /// <param name="type">The type to get the module name from.</param>
    /// <param name="namespacePart">The namespace part indicating the module.</param>
    /// <param name="splitIndex">The index to split the namespace to extract the module name.</param>
    /// <returns>The module name if available; otherwise, an empty string.</returns>
    public static string GetModuleName(this Type type, string namespacePart = "Module", int splitIndex = 2)
    {
        if (type?.Namespace is null)
        {
            return string.Empty;
        }

        return type.Namespace.Contains(namespacePart)
            ? type.Namespace.Split(".")[splitIndex].ToLowerInvariant()
            : string.Empty;
    }
}