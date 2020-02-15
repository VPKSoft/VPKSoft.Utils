﻿#region License
/*
VPKSoft.Utils

Some utilities by VPKSoft.
Copyright © 2020 VPKSoft, Petteri Kautonen

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
using System.ComponentModel;

namespace VPKSoft.Utils.XmlSettingsMisc
{
    /// <summary>
    /// Class RequestTypeConverterEventArgs.
    /// Implements the <see cref="System.EventArgs" />
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class RequestTypeConverterEventArgs: EventArgs
    {
        /// <summary>
        /// Gets or sets the name of the setting.
        /// </summary>
        public string SettingName { get; set; }

        /// <summary>
        /// Gets or sets the type to request a type converter for.
        /// </summary>
        public Type TypeToConvert { get; set; }

        /// <summary>
        /// Gets or sets the type converter for the setting.
        /// </summary>
        public TypeConverter TypeConverter { get; set; }
    }
}
