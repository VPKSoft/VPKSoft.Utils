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

namespace VPKSoft.Utils.Framework
{
    /// <summary>
    /// Some path utilities related to an "application".
    /// </summary>
    public class Paths
    {
        /// <summary>
        /// Just returns the default writable data directory for "non-roaming" applications.
        /// </summary>
        /// <returns>A writable data directory for "non-roaming" applications.</returns>
        public static string GetAppSettingsFolder()
        {
            if (!Utils.WPF)
            {
                return GetAppSettingsFolder(Misc.AppType.Winforms);
            }
            else
            {
                return GetAppSettingsFolder(Misc.AppType.WPF);
            }
        }


        /// <summary>
        /// Just returns the default writable data directory for "non-roaming" applications.
        /// </summary>
        /// <returns>A writable data directory for "non-roaming" applications.</returns>
        public static string GetAppSettingsFolder(Misc.AppType appType)
        {
            if (appType == Misc.AppType.Winforms)
            {
                string appName = System.Windows.Forms.Application.ProductName;
                if (appName == null || appName == string.Empty)
                {
                    appName = Path.GetFileNameWithoutExtension(System.AppDomain.CurrentDomain.FriendlyName);
                }
                foreach (char chr in Path.GetInvalidFileNameChars())
                {
                    appName = appName.Replace(chr, '_');
                }

                return Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\" + appName + @"\";
            }
            else if (appType == Misc.AppType.WPF)
            {
                string appName = ((AssemblyProductAttribute)Assembly.GetEntryAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false)[0]).Product;
                if (appName == null || appName == string.Empty)
                {
                    appName = Path.GetFileNameWithoutExtension(System.AppDomain.CurrentDomain.FriendlyName);
                }
                foreach (char chr in Path.GetInvalidFileNameChars())
                {
                    appName = appName.Replace(chr, '_');
                }

                return Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\" + appName + @"\";
            }
            else
            {
                // nice to throw something :-)
                throw new TypeInitializationException("Invalid application type.", new Exception());
            }
        }

        /// <summary>
        /// Generates and returns the default writable data directory for "non-roaming" applications.
        /// </summary>
        /// <returns>A writable data directory for "non-roaming" applications.</returns>
        public static string MakeAppSettingsFolder()
        {
            if (!Directory.Exists(GetAppSettingsFolder()))
            {
                Directory.CreateDirectory(GetAppSettingsFolder());
            }
            return GetAppSettingsFolder();
        }

        /// <summary>
        /// Generates and returns the default writable data directory for "non-roaming" applications.
        /// </summary>
        /// <returns>A writable data directory for "non-roaming" applications.</returns>
        public static string MakeAppSettingsFolder(Misc.AppType appType)
        {
            if (!Directory.Exists(GetAppSettingsFolder(appType)))
            {
                Directory.CreateDirectory(GetAppSettingsFolder(appType));
            }
            return GetAppSettingsFolder(appType);
        }
        /// <summary>
        /// Returns the executable directory of an application.
        /// </summary>
        public static string AppInstallDir
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory;
            }
        }
    }
}
