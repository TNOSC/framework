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

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Tnosc.Components.Abstractions.ApplicationService.Dispatchers;
using Tnosc.Components.Abstractions.Module;
using Tnosc.Components.Infrastructure.Api;
using Tnosc.Components.Infrastructure.ApplicationService.Dispatchers;
using Tnosc.Components.Infrastructure.ApplicationService.Commands;
using Tnosc.Components.Infrastructure.ApplicationService.Queries;
using Tnosc.Components.Infrastructure.ApplicationService.Events;

namespace Tnosc.Framework.Module.Core;
/// <summary>
/// Represents the core module of the application.
/// </summary>
public sealed class CoreModule : IModule
{
    /// <summary>
    /// Gets the name of the core module.
    /// </summary>
    public string Name => "Core";

    // Collection to store assemblies of the application
    private readonly ICollection<Assembly> _assemblies = new List<Assembly>();

    /// <summary>
    /// Sets the assemblies of the applicatiion.
    /// </summary>
    /// <param name="assemblies">The assemblies to set for the core module.</param>
    public void SetAssemblies(IEnumerable<Assembly> assemblies)
    {
        ArgumentNullException.ThrowIfNull(assemblies);

        // Add each assembly to the collection
        foreach (var assembly in assemblies)
        {
            _assemblies.Add(assembly);
        }
    }

    /// <summary>
    /// Registers services for the core module.
    /// </summary>
    /// <param name="services">The service collection to register services.</param>
    /// <param name="configuration">The configuration related to the core module.</param>
    public void Register(IServiceCollection services, IConfiguration configuration)
    {
        var disabledModules = new List<string>();

        // Check for disabled modules in the configuration
        foreach (var (key, value) in configuration.AsEnumerable())
        {
            if (!key.Contains(":module:enabled"))
            {
                continue;
            }

            if (!bool.Parse(value))
            {
                disabledModules.Add(key.Split(":")[0]);
            }
        }

        // Add core services
        services.AddMemoryCache();
        services.AddHttpClient();
        services.AddCommands(_assemblies);
        services.AddQueries(_assemblies);
        services.AddEvents(_assemblies);
        services.AddSingleton<IDispatcher, InMemoryDispatcher>();

        // Add controllers and configure application part manager
        services.AddControllers()
            .ConfigureApplicationPartManager(manager =>
            {
                var removedParts = new List<ApplicationPart>();

                // Remove parts related to disabled modules
                foreach (var disabledModule in disabledModules)
                {
                    var parts = manager.ApplicationParts.Where(x => x.Name.Contains(disabledModule,
                        StringComparison.InvariantCultureIgnoreCase));
                    removedParts.AddRange(parts);
                }

                // Remove the identified parts
                foreach (var part in removedParts)
                {
                    manager.ApplicationParts.Remove(part);
                }

                // Add custom feature provider for internal controllers
                manager.FeatureProviders.Add(new InternalControllerFeatureProvider());
            });
    }

    /// <summary>
    /// Configures middleware components for the core module.
    /// </summary>
    /// <param name="app">The application builder to configure middleware.</param>
    public void Use(IApplicationBuilder app)
    {
        // Currently, there is no specific middleware configuration for the core module
        // You can add middleware configuration specific to the core module here if needed
    }
}
