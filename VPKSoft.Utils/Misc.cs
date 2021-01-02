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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPKSoft.Utils
{
    /// <summary>
    /// Misc utilities.
    /// </summary>
    public class Misc
    {
        /// <summary>
        /// The application type. 
        /// <para/>WPF (for Windows Presentaton Foundation)
        /// <para/>Winforms (for Windows Forms application)
        /// <para/>Undefined (for throwing exceptions)
        /// </summary>
        public enum AppType
        {
            /// <summary>
            /// WPF (for Windows Presentaton Foundation)
            /// </summary>
            WPF,
            /// <summary>
            /// Winforms (for Windows Forms application)
            /// </summary>
            Winforms,
            /// <summary>
            /// And ASP.NET application
            /// </summary>
            ASP,
            /// <summary>
            /// Undefined (for throwing exceptions)
            /// </summary>
            Undefined
        };
    }
}
