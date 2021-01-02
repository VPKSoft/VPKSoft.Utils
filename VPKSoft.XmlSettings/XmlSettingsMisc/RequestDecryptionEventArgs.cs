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
    /// Event arguments for the <see cref="XmlSettings.RequestDecryption"/> event.
    /// Implements the <see cref="System.EventArgs" />
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class RequestDecryptionEventArgs: EventArgs
    {
        /// <summary>
        /// Gets or sets the name of the setting.
        /// </summary>
        public string SettingName { get; set; }

        /// <summary>
        /// Gets or sets the value of the setting to be decrypted from base64 encoded encrypted value.
        /// </summary>
        public string Value { get; set; }
    }
}
