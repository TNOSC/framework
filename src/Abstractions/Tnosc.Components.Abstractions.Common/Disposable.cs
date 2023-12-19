/*
 Copyright (c) 2023 Ahmed HEDFI (ahmed.hedfi@gmail.com)

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

namespace Tnosc.Components.Abstractions.Common;
/// <summary>
/// Base class for implementing the IDisposable pattern.
/// </summary>
public abstract class Disposable : IDisposable
{
    private bool _isDisposed;

    /// <summary>
    /// Finalizer (destructor) for ensuring that resources are released if Dispose is not called explicitly.
    /// </summary>
    ~Disposable()
    {
        Dispose(false);
    }

    /// <summary>
    /// Disposes of the resources held by the object.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Disposes of the managed and unmanaged resources.
    /// </summary>
    /// <param name="disposing">Indicates whether the method is called from the Dispose method.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!_isDisposed && disposing)
        {
            // Dispose of managed resources
            DisposeCore();
        }

        _isDisposed = true;
    }

    /// <summary>
    /// Implemented by derived classes to release managed resources.
    /// </summary>
    protected abstract void DisposeCore();
}