using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;

namespace Tnosc.Components.Abstractions.Module;

/// <summary>
/// Represents a modular component in the application.
/// </summary>
public interface IModule
{
    /// <summary>
    /// Gets the name of the module.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets the policies associated with the module.
    /// </summary>
    /// <remarks>
    /// Implementers can provide custom policies. Defaults to null if not overridden.
    /// </remarks>
    IEnumerable<string>? Policies => null;

    /// <summary>
    /// Registers services related to the module.
    /// </summary>
    /// <param name="services">The service collection to register services.</param>
    /// <param name="configuration">The configuration related to the module.</param>
    void Register(IServiceCollection services, IConfiguration configuration);

    /// <summary>
    /// Configures middleware components for the module.
    /// </summary>
    /// <param name="app">The application builder to configure middleware.</param>
    void Use(IApplicationBuilder app);
}