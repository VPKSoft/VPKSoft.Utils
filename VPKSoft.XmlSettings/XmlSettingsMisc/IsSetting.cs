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

namespace VPKSoft.Utils.XmlSettingsMisc
{
    /// <summary>
    /// An attribute to mark a setting property.
    /// Implements the <see cref="System.Attribute" />
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Property)]
    public class IsSetting : Attribute
    {
        /// <summary>
        /// Gets or sets a value whether to request encryption for the property value.
        /// </summary>
        public bool Secure { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has a specified default value.
        /// </summary>
        /// <value><c>true</c> if this instance has a specified default value; otherwise, <c>false</c>.</value>
        internal bool HasDefaultValue { get; set; }

        private object defaultValue;

        /// <summary>
        /// Gets or sets the default value of the setting.
        /// </summary>
        public object DefaultValue
        {
            get => defaultValue;

            set
            {
                defaultValue = value;
                HasDefaultValue = true;
            }
        }
    }
}
