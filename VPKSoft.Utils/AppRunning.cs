#region License
/*
VPKSoft.Utils

Some utilities by VPKSoft.
Copyright (C) 2016 VPKSoft, Petteri Kautonen

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private static List<Mutex> mutexes = new List<Mutex>();

        /// <summary>
        /// Checks if an application with a given unique string is already running.
        /// </summary>
        /// <param name="uniqueID">An (assumed) unique ID to use for the check.</param>
        /// <returns>True if an application with a given unique string is already running, otherwise false.</returns>
        public static bool CheckIfRunning(string uniqueID)
        {
            Mutex mutex;

            try
            {
                mutex = Mutex.OpenExisting(uniqueID);
                if (mutex != null)
                {
                    return true;
                }
                return false;
            }
            catch
            {
                mutex = new Mutex(true, uniqueID);
                mutexes.Add(mutex);
                return false;
            }
        }
    }
}
