#region License
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
using System.Diagnostics;

namespace VPKSoft.Utils
{
    /// <summary>
    /// An utility class to reserve, delete reservations and check reservations using the netsh command.
    /// <para/>This can help as you create self-hosted WCF http(s) services in the systems that do require permissions for them.
    /// </summary>
    public static class NetSH
    {
        /// <summary>
        /// Checks if an URL is reserved by using the netsh command.
        /// </summary>
        /// <param name="urlWildCard">An URL in to check. E.g. http(s)://+:991/foo/bar/</param>
        /// <param name="listen">If the command reported Yes to listen.</param>
        /// <param name="user">The user the command reported.</param>
        /// <returns>True if the URL is reserved, otherwise false. On an exception a null value is returned.</returns>
        public static bool? IsNetShUrlReserved(string urlWildCard, out bool listen, out string user)
        {
            try
            {
                Process p = new Process();
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.FileName = "netsh";
                p.StartInfo.Arguments = string.Format("http show urlacl url={0}", urlWildCard);
                p.Start();
                string[] output = p.StandardOutput.ReadToEnd().Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                p.WaitForExit();

                bool retval = false;
                user = string.Empty;
                listen = false;

                foreach (string line in output)
                {
                    if (line.Contains("Reserved URL") && line.Contains(urlWildCard))
                    {
                        retval = true;
                    }
                    else if (line.Contains("User:"))
                    {
                        user = line.Replace("User:", string.Empty).Trim();
                    }
                    else if (line.Contains("Listen:"))
                    {
                        string tmp = line.Replace("Listen:", string.Empty).Trim();
                        listen = tmp == "Yes";
                    }
                }
                return retval;
            }
            catch
            {
                listen = false;
                user = string.Empty;
                return null;
            }
        }

        /// <summary>
        /// Checks if an url is reserved by using the netsh command.
        /// </summary>
        /// <param name="urlWildCard">An url in to check. E.g. http(s)://+:991/foo/bar/</param>
        /// <returns>True if the url is reserved, otherwise false. On an exception a null value is returned.</returns>
        public static bool? IsNetShUrlReserved(string urlWildCard)
        {
            bool listen;
            string user;
            return IsNetShUrlReserved(urlWildCard, out listen, out user);
        }

        /// <summary>
        /// Reserves an url by using the netsh command.
        /// </summary>
        /// <param name="urlWildCard">An url to reserve. E.g. http(s)://+:991/foo/bar/</param>
        /// <param name="user">The user who should get the permission to use the reservation. The default is Everyone.</param>
        /// <remarks>
        /// This requires elevation (run as Administrator).
        /// </remarks>
        /// <returns>True if the url reservation was a success, otherwise false. On an exception a null value is returned.</returns>
        public static bool? ReserveNetShUrl(string urlWildCard, string user = "Everyone")
        {
            try
            {
                Process p = new Process();
                p.StartInfo.UseShellExecute = true;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.FileName = "netsh";
                p.StartInfo.Arguments = string.Format("http add urlacl url={0} user={1}", urlWildCard, user);
                p.StartInfo.Verb = "runas"; // Need to be admin to do this...
                p.Start();
                p.WaitForExit();
                return p.ExitCode == 0;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Removes a reserved url by using the netsh command.
        /// </summary>
        /// <param name="urlWildCard">An url in to remove a reservation from. E.g. http(s)://+:991/foo/bar/</param>
        /// <remarks>
        /// This requires elevation (run as Administrator).
        /// </remarks>
        /// <returns>True if the url reservation was removed successfully, otherwise false. On an exception a null value is returned.</returns>
        public static bool? RemoveReservedNetShUrl(string urlWildCard)
        {
            try
            {
                Process p = new Process();
                p.StartInfo.UseShellExecute = true;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.FileName = "netsh";
                p.StartInfo.Arguments = string.Format("http delete urlacl url={0}", urlWildCard);
                p.StartInfo.Verb = "runas"; // Need to be admin to do this...
                p.Start();
                p.WaitForExit();
                return p.ExitCode == 0;
            }
            catch
            {
                return null;
            }
        }

    }
}
