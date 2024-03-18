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

using System.Reflection;
using Microsoft.Extensions.Configuration;
using Tnosc.Components.Abstractions.Module;

namespace Tnosc.Components.Infrastructure.Module;
/// <summary>
/// Utility class for loading assemblies and extracting modules.
/// </summary>
public static class ModuleLoader
{
    /// <summary>
    /// Loads assemblies based on the provided configuration and module part.
    /// </summary>
    /// <param name="configuration">The configuration containing module-related settings.</param>
    /// <param name="modulePart">The part of the module's file name to identify relevant assemblies.</param>
    /// <returns>A list of loaded assemblies.</returns>
    public static IList<Assembly> LoadAssemblies(IConfiguration configuration, string modulePart)
    {

        var assemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();

        // Get locations of already loaded assemblies
        var locations = assemblies.Where(x => !x.IsDynamic).Select(x => x.Location).ToArray();

        // Get all DLL files in the application's base directory
        var files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll")
            .Where(x => !locations.Contains(x, StringComparer.InvariantCultureIgnoreCase))
            .ToList();

        var disabledModules = new List<string>();

        // Iterate through DLL files to filter by modulePart and check for module enabling
        foreach (var file in files)
        {
            if (!file.Contains(modulePart))
            {
                continue;
            }

            var moduleName = file.Split(modulePart)[1].Split(".")[0].ToLowerInvariant();
            var enabled = configuration.GetValue<bool>($"{moduleName}:module:enabled");

            if (!enabled)
            {
                disabledModules.Add(file);
            }
        }

        // Remove disabled modules from the list of files
        foreach (var disabledModule in disabledModules)
        {
            files.Remove(disabledModule);
        }

        // Load remaining DLL files as assemblies
        files.ForEach(x => assemblies.Add(AppDomain.CurrentDomain.Load(AssemblyName.GetAssemblyName(x))));

        return assemblies;
    }

    /// <summary>
    /// Loads modules from the provided assemblies.
    /// </summary>
    /// <param name="assemblies">The list of assemblies containing modules.</param>
    /// <returns>A list of loaded modules implementing the IModule interface.</returns>
    public static IList<IModule> LoadModules(IEnumerable<Assembly> assemblies)
        => assemblies
            .SelectMany(x => x.GetTypes())
            .Where(x => typeof(IModule).IsAssignableFrom(x) && !x.IsInterface)
            .OrderBy(x => x.Name)
            .Select(Activator.CreateInstance)
            .Cast<IModule>()
            .ToList();
}
