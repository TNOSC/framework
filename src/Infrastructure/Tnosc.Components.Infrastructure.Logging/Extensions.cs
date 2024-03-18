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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Filters;
using Tnosc.Components.Infrastructure.Common;
using Tnosc.Components.Abstractions.ApplicationService.Commands;
using Tnosc.Components.Abstractions.ApplicationService.Events;
using Tnosc.Components.Abstractions.ApplicationService.Queries;
using Tnosc.Components.Abstractions.Context;
using Tnosc.Components.Infrastructure.Logging.Decorators;
using Tnosc.Components.Infrastructure.Logging.Options;
using FileOptions = Tnosc.Components.Infrastructure.Logging.Options.FileOptions;

namespace Tnosc.Components.Infrastructure.Logging;
/// <summary>
/// Provides extension methods for configuring logging and adding logging decorators to services.
/// </summary>
public static class Extensions
{
    private const string AppSectionName = "app";
    private const string ConsoleOutputTemplate = "{Timestamp:HH:mm:ss} [{Level:u3}] {Message}{NewLine}{Exception}";
    private const string FileOutputTemplate = "{Timestamp:HH:mm:ss} [{Level:u3}] ({SourceContext}.{Method}) {Message}{NewLine}{Exception}";
    private const string LoggerSectionName = "logger";

    /// <summary>
    /// Adds logging decorators for command, event, and query handlers.
    /// </summary>
    public static IServiceCollection AddLoggingDecorators(this IServiceCollection services)
    {
        services.TryDecorate(typeof(ICommandHandler<>), typeof(LoggingCommandHandlerDecorator<>));
        services.TryDecorate(typeof(IEventHandler<>), typeof(LoggingEventHandlerDecorator<>));
        services.TryDecorate(typeof(IQueryHandler<,>), typeof(LoggingQueryHandlerDecorator<,>));

        return services;
    }

    /// <summary>
    /// Retrieves application options from the provided configuration.
    /// </summary>
    public static AppOptions GetAppOptions(this IConfiguration configuration)
      => configuration.GetOptions<AppOptions>(AppSectionName);

    /// <summary>
    /// Adds middleware for logging requests and responses.
    /// </summary>
    public static IApplicationBuilder UseLogging(this IApplicationBuilder app)
    {
        app.Use(async (ctx, next) =>
        {
            var logger = ctx.RequestServices.GetRequiredService<ILogger<IContext>>();
            var context = ctx.RequestServices.GetRequiredService<IContext>();
            logger.LogInformation("Started processing a request [Request ID: '{RequestId}', Correlation ID: '{CorrelationId}', Trace ID: '{TraceId}', User ID: '{UserId}']...",
                context.RequestId, context.CorrelationId, context.TraceId, context.Identity!.IsAuthenticated ? context.Identity.Id : string.Empty);

            await next();

            logger.LogInformation("Finished processing a request with status code: {StatusCode} [Request ID: '{RequestId}', Correlation ID: '{CorrelationId}', Trace ID: '{TraceId}', User ID: '{UserId}']",
                ctx.Response.StatusCode, context.RequestId, context.CorrelationId, context.TraceId, context.Identity.IsAuthenticated ? context.Identity.Id : string.Empty);
        });

        return app;
    }

    /// <summary>
    /// Configures logging for the host builder.
    /// </summary>
    public static IHostBuilder UseLogging(
        this IHostBuilder builder,
        Action<LoggerConfiguration>? configure = null,
        string loggerSectionName = LoggerSectionName)
    {
        return builder.UseSerilog((context, loggerConfiguration) =>
        {
            if (string.IsNullOrWhiteSpace(loggerSectionName))
            {
                loggerSectionName = LoggerSectionName;
            }

            var appOptions = context.Configuration.GetAppOptions();
            var loggerOptions = context.Configuration.GetOptions<LoggerOptions>(loggerSectionName);

            MapOptions(loggerOptions, appOptions, loggerConfiguration, context.HostingEnvironment.EnvironmentName);
            configure?.Invoke(loggerConfiguration);
        });
    }

    private static void MapOptions(
        LoggerOptions loggerOptions,
        AppOptions appOptions,
        LoggerConfiguration loggerConfiguration,
        string environmentName)
    {
        var level = GetLogEventLevel(loggerOptions.Level);

        loggerConfiguration.Enrich.FromLogContext()
            .MinimumLevel.Is(level)
            .Enrich.WithProperty("Environment", environmentName)
            .Enrich.WithProperty("Application", appOptions.Name)
            .Enrich.WithProperty("Instance", appOptions.Instance)
            .Enrich.WithProperty("Version", appOptions.Version);

        foreach (var (key, value) in loggerOptions.Tags ?? new Dictionary<string, object>())
        {
            loggerConfiguration.Enrich.WithProperty(key, value);
        }

        foreach (var (key, value) in loggerOptions.Overrides ?? new Dictionary<string, string>())
        {
            var logLevel = GetLogEventLevel(value);
            loggerConfiguration.MinimumLevel.Override(key, logLevel);
        }

        loggerOptions.ExcludePaths?.ToList().ForEach(p => loggerConfiguration.Filter
            .ByExcluding(Matching.WithProperty<string>("RequestPath", n => n.EndsWith(p))));

        loggerOptions.ExcludeProperties?.ToList().ForEach(p => loggerConfiguration.Filter
            .ByExcluding(Matching.WithProperty(p)));

        Configure(loggerConfiguration, loggerOptions);
    }

    private static void Configure(LoggerConfiguration loggerConfiguration, LoggerOptions options)
    {
        var consoleOptions = options.Console ?? new ConsoleOptions();
        var fileOptions = options.File ?? new FileOptions();
        var seqOptions = options.Seq ?? new SeqOptions();

        if (consoleOptions.Enabled)
        {
            loggerConfiguration.WriteTo.Console(outputTemplate: ConsoleOutputTemplate);
        }

        if (fileOptions.Enabled)
        {
            var path = string.IsNullOrWhiteSpace(fileOptions.Path) ? "logs/logs.txt" : fileOptions.Path;
            if (!Enum.TryParse<RollingInterval>(fileOptions.Interval, true, out var interval))
            {
                interval = RollingInterval.Day;
            }

            loggerConfiguration.WriteTo.File(path, rollingInterval: interval, outputTemplate: FileOutputTemplate);
        }

        if (seqOptions.Enabled)
        {
            loggerConfiguration.WriteTo.Seq(seqOptions.Url, apiKey: seqOptions.ApiKey);
        }
    }

    private static LogEventLevel GetLogEventLevel(string level)
        => Enum.TryParse<LogEventLevel>(level, true, out var logLevel)
            ? logLevel
            : LogEventLevel.Information;
}

