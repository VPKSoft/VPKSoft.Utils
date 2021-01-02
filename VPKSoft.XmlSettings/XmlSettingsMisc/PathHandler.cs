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

using System;
using System.IO;
using System.Reflection;

namespace VPKSoft.Utils.XmlSettingsMisc
{
    /// <summary>
    /// Path utilities for the <see cref="XmlSettings"/> class.
    /// </summary>
    public class PathHandler
    {
        /// <summary>
        /// Creates a setting path for a given <see cref="Environment.SpecialFolder"/> enumeration value and for a
        /// given assembly name if it doesn't already exist and returns the created or existing folder path.
        /// </summary>
        /// <param name="assemblyName">Name of the assembly.</param>
        /// <param name="folder">An enumerated constant that identifies a system special folder.</param>
        /// <returns>A path to a requested folder.</returns>
        public static string GetCreateSettingPath(string assemblyName, Environment.SpecialFolder folder)
        {
            var path = 
                Path.Combine(Environment.GetFolderPath(folder),
                assemblyName);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path;
        }

        // <summary>Gets the full name of the assembly, also known as the display name.</summary>
        // <returns>A string that is the full name of the assembly, also known as the display name.</returns>

        /// <summary>
        /// Creates a setting path for a given <see cref="Environment.SpecialFolder"/> enumeration value and for a
        /// given assembly if it doesn't already exist and returns the created or existing folder path.
        /// </summary>
        /// <param name="assembly">The assembly to get the name from.</param>
        /// <param name="folder">An enumerated constant that identifies a system special folder.</param>
        /// <returns>A path to a requested folder.</returns>
        public static string GetCreateSettingPath(Assembly assembly, Environment.SpecialFolder folder)
        {
            return GetCreateSettingPath(assembly.GetName().Name, folder);
        }

        /// <summary>
        /// Deletes a setting path for a given <see cref="Environment.SpecialFolder"/> enumeration value and for a
        /// given assembly name.
        /// </summary>
        /// <param name="assemblyName">Name of the assembly.</param>
        /// <param name="folder">An enumerated constant that identifies a system special folder.</param>
        /// <returns><c>true</c> if the directory was successfully deleted, <c>false</c> otherwise.</returns>
        public static bool DeleteSettingDirectory(string assemblyName, Environment.SpecialFolder folder)
        {
            try
            {
                var path = 
                    Path.Combine(Environment.GetFolderPath(folder),
                        assemblyName);

                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Deletes a setting path for a given <see cref="Environment.SpecialFolder"/> enumeration value and for a
        /// given assembly.
        /// </summary>
        /// <param name="assembly">The assembly to get the name from.</param>
        /// <param name="folder">An enumerated constant that identifies a system special folder.</param>
        /// <returns><c>true</c> if the directory was successfully deleted, <c>false</c> otherwise.</returns>
        public static bool DeleteSettingDirectory(Assembly assembly, Environment.SpecialFolder folder)
        {
            return DeleteSettingDirectory(assembly.GetName().Name, folder);
        }

        /// <summary>
        /// Creates a setting file path for a given <see cref="Environment.SpecialFolder"/> enumeration value and for a
        /// given assembly name if it doesn't already exist and returns the created or existing folder path and a file name.
        /// </summary>
        /// <param name="assemblyName">Name of the assembly.</param>
        /// <param name="fileExtension">The file extension to use. I.e. 'xml'.</param>
        /// <param name="folder">An enumerated constant that identifies a system special folder.</param>
        /// <returns>A path to a requested file.</returns>
        public static string GetSettingsFile(string assemblyName, string fileExtension, Environment.SpecialFolder folder)
        {
            fileExtension = fileExtension.TrimStart('*', '.');
            return Path.Combine(GetCreateSettingPath(assemblyName, folder), assemblyName + "." + fileExtension);
        }

        /// <summary>
        /// Creates a setting file path for a given <see cref="Environment.SpecialFolder"/> enumeration value and for a
        /// given assembly if it doesn't already exist and returns the created or existing folder path and a file name.
        /// </summary>
        /// <param name="assembly">The assembly to get the name from.</param>
        /// <param name="fileExtension">The file extension to use. I.e. 'xml'.</param>
        /// <param name="folder">An enumerated constant that identifies a system special folder.</param>
        /// <returns>A path to a requested file.</returns>
        public static string GetSettingsFile(Assembly assembly, string fileExtension, Environment.SpecialFolder folder)
        {
            return GetSettingsFile(assembly.GetName().Name, fileExtension, folder);
        }
    }
}
