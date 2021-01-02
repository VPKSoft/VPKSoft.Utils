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

namespace VPKSoft.Utils
{
    /// <summary>
    /// Utilities by VPKSoft.
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// True if the application is a ASP.NET application.
        /// </summary>
        public static bool ASP
        {
            get
            {
                return System.Web.HttpContext.Current == null;
            }
        }

        /// <summary>
        /// True if the application is a Windows Forms Application
        /// </summary>
        public static bool WinForms
        {
            get
            {
                return System.Windows.Forms.Application.OpenForms.Count == 0;
            }
        }

        /// <summary>
        /// True if the application is a Windows Presentation Foundation application
        /// </summary>
        public static bool WPF
        {
            get
            {
                return System.Windows.Application.Current != null;
            }
        }
    }
}
