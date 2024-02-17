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

namespace Tnosc.Components.Abstractions.Common.Results;
/// <summary>
/// Represents the type of error that can occur in the application.
/// </summary>
public enum ErrorType
{
    NotFound,   // Indicates a resource was not found
    Validation, // Indicates a validation error
    Failure,    // Indicates a general failure
    Conflict,   // Indicates a conflict with existing data
    Forbidden,  // Indicates an action is forbidden or not allowed
    Unauthorized, // Indicates unauthorized access or authentication failure
}
