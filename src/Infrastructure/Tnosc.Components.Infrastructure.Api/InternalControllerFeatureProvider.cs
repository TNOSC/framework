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

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Reflection;

namespace Tnosc.Components.Infrastructure.Api;
/// <summary>
/// Custom feature provider for ASP.NET Core controllers.
/// </summary>
public class InternalControllerFeatureProvider : ControllerFeatureProvider
{
    /// <summary>
    /// Determines whether the specified type should be considered a controller.
    /// </summary>
    /// <param name="typeInfo">The TypeInfo representing the type to check.</param>
    /// <returns>True if the type should be considered a controller; otherwise, false.</returns>
    protected override bool IsController(TypeInfo typeInfo)
    {
        // Exclude non-class types
        if (!typeInfo.IsClass)
        {
            return false;
        }

        // Exclude abstract classes
        if (typeInfo.IsAbstract)
        {
            return false;
        }

        // Exclude generic types
        if (typeInfo.ContainsGenericParameters)
        {
            return false;
        }

        // Exclude types decorated with NonControllerAttribute
        if (typeInfo.IsDefined(typeof(NonControllerAttribute)))
        {
            return false;
        }

        // Include types ending with "Controller" or decorated with ControllerAttribute
        if (!typeInfo.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase) &&
            !typeInfo.IsDefined(typeof(ControllerAttribute)))
        {
            return false;
        }

        return true;
    }
}
