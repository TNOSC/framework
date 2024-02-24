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
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tnosc.Components.Abstractions.Module;

namespace Tnosc.Components.Infrastructure.Module;
/// <summary>
/// Extension methods for dependency injection and module configuration.
/// </summary>
public static class DependencyInjectionExtensions
{
    private static readonly string CoreModuleName = "Core";

    /// <summary>
    /// Adds modules to the service collection and configures them.
    /// </summary>
    /// <param name="services">The service collection to add modules to.</param>
    /// <param name="configuration">The configuration for the application.</param>
    /// <param name="assemblies">The list of assemblies containing modules.</param>
    /// <param name="modules">The list of modules to add and configure.</param>
    /// <returns>The modified service collection.</returns>
    public static IServiceCollection AddModules(this IServiceCollection services,
        IConfiguration configuration, IList<Assembly> assemblies, IList<IModule> modules)
    {
        foreach (var module in modules)
        {
            // Special handling for the Core module to set assemblies
            if (module.Name == CoreModuleName)
                SetAssemblies(module, assemblies);

            // Register services for the module
            module.Register(services, configuration);
        }
        return services;
    }

    /// <summary>
    /// Invokes the "SetAssemblies" method on the Core module to set assemblies.
    /// </summary>
    /// <param name="module">The Core module instance.</param>
    /// <param name="assemblies">The list of assemblies to set.</param>
    private static void SetAssemblies(IModule module, IList<Assembly> assemblies)
    {
        ArgumentNullException.ThrowIfNull(module);
        ArgumentNullException.ThrowIfNull(assemblies);

        // Use reflection to invoke the "SetAssemblies" method on the Core module
        MethodInfo methodInfo = module.GetType().GetMethod("SetAssemblies") ?? throw new InvalidOperationException("SetAssemblies is not found in Core Module");
        methodInfo.Invoke(module, new object[] { assemblies });
    }

    /// <summary>
    /// Configures modules in the application by adding configuration sources.
    /// </summary>
    /// <param name="builder">The web host builder.</param>
    /// <returns>The modified web host builder.</returns>
    public static IWebHostBuilder ConfigureModules(this IWebHostBuilder builder)
    {
        return builder.ConfigureAppConfiguration((ctx, cfg) =>
        {
            // Add configuration sources for module settings files
            foreach (var settings in GetSettings("*"))
            {
                cfg.AddJsonFile(settings);
            }

            foreach (var settings in GetSettings($"*.{ctx.HostingEnvironment.EnvironmentName}"))
            {
                cfg.AddJsonFile(settings);
            }

            IEnumerable<string> GetSettings(string pattern)
                => Directory.EnumerateFiles(ctx.HostingEnvironment.ContentRootPath,
                    $"module.{pattern}.json", SearchOption.AllDirectories);
        });
    }
}
