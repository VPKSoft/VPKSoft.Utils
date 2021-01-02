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
using System.Drawing;


namespace VPKSoft.Utils
{
    /// <summary>
    /// An utility class to get common system icons.
    /// </summary>
    public static class SysIcons
    {
        /// <summary>
        /// An enumeration of the SystemIcons class icons.
        /// </summary>
        public enum SystemIconType
        {
            /// <summary>
            /// An Icon object that contains the default application icon (WIN32: IDI_APPLICATION).
            /// </summary>
            Application,

            /// <summary>
            /// An Icon object that contains the system asterisk icon (WIN32: IDI_ASTERISK).
            /// </summary>
            Asterisk,

            /// <summary>
            /// An Icon object that contains the system error icon (WIN32: IDI_ERROR).
            /// </summary>
            Error,
            
            /// <summary>
            /// An Icon object that contains the system exclamation icon (WIN32: IDI_EXCLAMATION).
            /// </summary>
            Exclamation,

            /// <summary>
            /// An Icon object that contains the system hand icon (WIN32: IDI_HAND).
            /// </summary>
            Hand,

            /// <summary>
            /// An Icon object that contains the system information icon (WIN32: IDI_INFORMATION).
            /// </summary>
            Information,

            /// <summary>
            /// An Icon object that contains the system question icon (WIN32: IDI_QUESTION).
            /// </summary>
            Question,

            /// <summary>
            /// An Icon object that contains the shield icon.
            /// </summary>
            Shield,

            /// <summary>
            /// An Icon object that contains the system warning icon (WIN32: IDI_WARNING).
            /// </summary>
            Warning,

            /// <summary>
            /// An Icon object that contains the Windows logo icon (WIN32: IDI_WINLOGO).
            /// </summary>
            WinLogo
        }

        /// <summary>
        /// Gets a system Icon based of the given <paramref name="type"/> enumeration.
        /// </summary>
        /// <param name="type">A SystemIconType enumeration for the icon.</param>
        /// <returns>The requested icon or null if it was't found.</returns>
        public static Icon GetSystemIcon(SystemIconType type)
        {
            switch (type)
            {
                case SystemIconType.Application:
                    return SystemIcons.Application;
                case SystemIconType.Asterisk:
                    return SystemIcons.Asterisk;
                case SystemIconType.Error:
                    return SystemIcons.Error;
                case SystemIconType.Exclamation:
                    return SystemIcons.Exclamation;
                case SystemIconType.Hand:
                    return SystemIcons.Hand;
                case SystemIconType.Information:
                    return SystemIcons.Information;
                case SystemIconType.Question:
                    return SystemIcons.Question;
                case SystemIconType.Shield:
                    return SystemIcons.Shield;
                case SystemIconType.Warning:
                    return SystemIcons.Warning;
                case SystemIconType.WinLogo:
                    return SystemIcons.WinLogo;
                default: return null;
            }
        }

        /// <summary>
        /// Gets a system Icon based of the given <paramref name="type"/> enumeration as a Bitmap. 
        /// </summary>
        /// <param name="type">A SystemIconType enumeration for the icon.</param>
        /// <returns>The requested icon as a Bitmap.</returns>
        public static Bitmap GetSystemIconBitmap(SystemIconType type)
        {
            return GetSystemIcon(type).ToBitmap();
        }

        /// <summary>
        /// Gets a system Icon based of the given <paramref name="type"/> enumeration as a Bitmap. 
        /// </summary>
        /// <param name="type">A SystemIconType enumeration for the icon.</param>
        /// <param name="size">A size for the Bitmap for scaling the icon.</param>
        /// <returns>The requested icon as a Bitmap.</returns>
        public static Bitmap GetSystemIconBitmap(SystemIconType type, Size size)
        {
            Icon ico = GetSystemIcon(type);
            Bitmap bm = new Bitmap(size.Width, size.Height);

            using (Graphics g = Graphics.FromImage(bm))
            {
                g.DrawIcon(ico, new Rectangle(0, 0, size.Width, size.Height));
            }

            return bm;
        }

    }
}
