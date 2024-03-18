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

namespace Tnosc.Components.Infrastructure.Logging.Options;
/// <summary>
/// Represents options for configuring logging.
/// </summary>
public class LoggerOptions
{
    /// <summary>
    /// Gets or sets the logging level.
    /// </summary>
    public string Level { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the options for console logging.
    /// </summary>
    public ConsoleOptions? Console { get; set; }

    /// <summary>
    /// Gets or sets the options for file logging.
    /// </summary>
    public FileOptions? File { get; set; }

    /// <summary>
    /// Gets or sets the options for Seq logging.
    /// </summary>
    public SeqOptions? Seq { get; set; }

    /// <summary>
    /// Gets or sets the dictionary of overrides for logging configuration.
    /// </summary>
    public IDictionary<string, string>? Overrides { get; set; }

    /// <summary>
    /// Gets or sets the collection of paths to be excluded from logging.
    /// </summary>
    public IEnumerable<string>? ExcludePaths { get; set; }

    /// <summary>
    /// Gets or sets the collection of properties to be excluded from logging.
    /// </summary>
    public IEnumerable<string>? ExcludeProperties { get; set; }

    /// <summary>
    /// Gets or sets the dictionary of tags for logging.
    /// </summary>
    public IDictionary<string, object>? Tags { get; set; }
}

