#region License
/*
VPKSoft.Utils

Some utilities by VPKSoft.
Copyright © 2021 VPKSoft, Petteri Kautonen

Contact: vpksoft@vpksoft.net

This file is part of VPKSoft.Utils.

VPKSoft.Utils is free software: you can redistribute it and/or modify
it under the terms of the GNU Lesser General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

VPKSoft.Utils is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License
along with VPKSoft.Utils.  If not, see <http://www.gnu.org/licenses/>.
*/
#endregion

using System.Collections.Generic;
using System.Threading;

namespace VPKSoft.Utils
{
    /// <summary>
    /// Application utilities, for example to check if an application is already running.
    /// </summary>
    public static class AppRunning
    {
        /// <summary>
        /// A static list to hold the created mutexes.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        private static readonly List<Mutex> mutexes = new List<Mutex>();

        /// <summary>
        /// Checks if an application with a given unique string is already running.
        /// </summary>
        /// <param name="uniqueID">An (assumed) unique ID to use for the check.</param>
        /// <returns>True if an application with a given unique string is already running, otherwise false.</returns>
        // ReSharper disable once InconsistentNaming
        public static bool CheckIfRunning(string uniqueID)
        {
            try
            {
                Mutex.OpenExisting(uniqueID);
                return true;
            }
            catch
            {
                var mutex = new Mutex(true, uniqueID);
                mutexes.Add(mutex);
                return false;
            }
        }

        /// <summary>
        /// Disposes a mutex reserved by an application if one exists in the internal collection.
        /// </summary>
        /// <param name="uniqueID">An (assumed) unique ID for the mutex to dispose of.</param>
        // ReSharper disable once InconsistentNaming
        public static void DisposeMutexByName(string uniqueID)
        {
            try
            {
                var mutex = Mutex.OpenExisting(uniqueID);
                int index = mutexes.IndexOf(mutex);
                if (index != -1)
                {
                    using (mutex)
                    {
                        mutexes.RemoveAt(index);
                    }
                }
            }
            catch
            {
                // there is no mutex with a given name..
            }
        }

        /// <summary>
        /// Checks if an application with a given unique string is already running but doesn't create a new <see cref="Mutex"/> with the given <paramref name="uniqueID"/> name.
        /// </summary>
        /// <param name="uniqueID">An (assumed) unique ID to use for the check.</param>
        /// <returns>True if an application with a given unique string is already running, otherwise false.</returns>
        // ReSharper disable once InconsistentNaming
        public static bool CheckIfRunningNoAdd(string uniqueID)
        {
            try
            {
                Mutex.OpenExisting(uniqueID);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
