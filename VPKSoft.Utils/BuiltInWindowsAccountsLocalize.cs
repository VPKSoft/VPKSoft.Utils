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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPKSoft.Utils
{
    /// <summary>
    /// A class to help localization of Windows built-in user names.
    /// </summary>
    public static class BuiltInWindowsAccountsLocalize
    {
        #region WindowsAccountsSIDs
        /// <summary>
        /// \NULL SID
        /// </summary>
        public const string NULL_SID = "S-1-0-0";

        /// <summary>
        /// \Everyone
        /// </summary>
        public const string Everyone = "S-1-1-0";

        /// <summary>
        /// \LOCAL
        /// </summary>
        public const string LOCAL = "S-1-2-0";

        /// <summary>
        /// \CONSOLE LOGON
        /// </summary>
        public const string CONSOLE_LOGON = "S-1-2-1";

        /// <summary>
        /// \CREATOR OWNER
        /// </summary>
        public const string CREATOR_OWNER = "S-1-3-0";

        /// <summary>
        /// \CREATOR GROUP
        /// </summary>
        public const string CREATOR_GROUP = "S-1-3-1";

        /// <summary>
        /// \CREATOR OWNER SERVER
        /// </summary>
        public const string CREATOR_OWNER_SERVER = "S-1-3-2";

        /// <summary>
        /// \CREATOR GROUP SERVER
        /// </summary>
        public const string CREATOR_GROUP_SERVER = "S-1-3-3";

        /// <summary>
        /// \OWNER RIGHTS
        /// </summary>
        public const string OWNER_RIGHTS = "S-1-3-4";

        /// <summary>
        /// NT SERVICE\ALL SERVICES
        /// </summary>
        public const string NTSERV_ALL_SERVICES = "S-1-5-80-0";

        /// <summary>
        /// NT SERVICE\NT SERVICE
        /// </summary>
        public const string NTSERV_NT_SERVICE = "S-1-5-80";

        /// <summary>
        /// NT AUTHORITY\DIALUP
        /// </summary>
        public const string NTAUTH_DIALUP = "S-1-5-1";

        /// <summary>
        /// NT AUTHORITY\NETWORK
        /// </summary>
        public const string NTAUTH_NETWORK = "S-1-5-2";

        /// <summary>
        /// NT AUTHORITY\BATCH
        /// </summary>
        public const string NTAUTH_BATCH = "S-1-5-3";

        /// <summary>
        /// NT AUTHORITY\INTERACTIVE
        /// </summary>
        public const string NTAUTH_INTERACTIVE = "S-1-5-4";

        /// <summary>
        /// NT AUTHORITY\SERVICE
        /// </summary>
        public const string NTAUTH_SERVICE = "S-1-5-6";

        /// <summary>
        /// NT AUTHORITY\ANONYMOUS LOGON
        /// </summary>
        public const string NTAUTH_ANONYMOUS_LOGON = "S-1-5-7";

        /// <summary>
        /// NT AUTHORITY\PROXY
        /// </summary>
        public const string NTAUTH_PROXY = "S-1-5-8";

        /// <summary>
        /// NT AUTHORITY\ENTERPRISE DOMAIN CONTROLLERS
        /// </summary>
        public const string NTAUTH_ENTERPRISE_DOMAIN_CONTROLLERS = "S-1-5-9";

        /// <summary>
        /// NT AUTHORITY\SELF
        /// </summary>
        public const string NTAUTH_SELF = "S-1-5-10";

        /// <summary>
        /// NT AUTHORITY\Authenticated Users
        /// </summary>
        public const string NTAUTH_Authenticated_Users = "S-1-5-11";

        /// <summary>
        /// NT AUTHORITY\RESTRICTED
        /// </summary>
        public const string NTAUTH_RESTRICTED = "S-1-5-12";

        /// <summary>
        /// NT AUTHORITY\TERMINAL SERVER USER
        /// </summary>
        public const string NTAUTH_TERMINAL_SERVER_USER = "S-1-5-13";

        /// <summary>
        /// NT AUTHORITY\REMOTE INTERACTIVE LOGON
        /// </summary>
        public const string NTAUTH_REMOTE_INTERACTIVE_LOGON = "S-1-5-14";

        /// <summary>
        /// NT AUTHORITY\This Organization
        /// </summary>
        public const string NTAUTH_This_Organization = "S-1-5-15";

        /// <summary>
        /// NT AUTHORITY\IUSR
        /// </summary>
        public const string NTAUTH_IUSR = "S-1-5-17";

        /// <summary>
        /// NT AUTHORITY\SYSTEM
        /// </summary>
        public const string NTAUTH_SYSTEM = "S-1-5-18";

        /// <summary>
        /// NT AUTHORITY\LOCAL SERVICE
        /// </summary>
        public const string NTAUTH_LOCAL_SERVICE = "S-1-5-19";

        /// <summary>
        /// NT AUTHORITY\NETWORK SERVICE
        /// </summary>
        public const string NTAUTH_NETWORK_SERVICE = "S-1-5-20";

        /// <summary>
        /// NT AUTHORITY\NTLM Authentication
        /// </summary>
        public const string NTAUTH_NTLM_Authentication = "S-1-5-64-10";

        /// <summary>
        /// NT AUTHORITY\SChannel Authentication
        /// </summary>
        public const string NTAUTH_SChannel_Authentication = "S-1-5-64-14";

        /// <summary>
        /// NT AUTHORITY\Digest Authentication
        /// </summary>
        public const string NTAUTH_Digest_Authentication = "S-1-5-64-21";

        /// <summary>
        /// Unknown	Mandatory Label\Untrusted Mandatory Level
        /// </summary>
        public const string UML_Untrusted_Mandatory_Level = "S-1-16-0";

        /// <summary>
        /// Unknown	Mandatory Label\Low Mandatory Level
        /// </summary>
        public const string UML_Low_Mandatory_Level = "S-1-16-4096";

        /// <summary>
        /// Unknown	Mandatory Label\Medium Mandatory Level
        /// </summary>
        public const string UML_Medium_Mandatory_Level = "S-1-16-8192";

        /// <summary>
        /// Unknown	Mandatory Label\Medium Plus Mandatory Level
        /// </summary>
        public const string UML_Medium_Plus_Mandatory_Level = "S-1-16-8448";

        /// <summary>
        /// Unknown	Mandatory Label\High Mandatory Level
        /// </summary>
        public const string UML_High_Mandatory_Level = "S-1-16-12288";

        /// <summary>
        /// Unknown	Mandatory Label\System Mandatory Level
        /// </summary>
        public const string UML_System_Mandatory_Level = "S-1-16-16384";

        /// <summary>
        /// Unknown	Mandatory Label\Protected Process Mandatory Level
        /// </summary>
        public const string UML_Protected_Process_Mandatory_Level = "S-1-16-20480";

        /// <summary>
        /// BUILTIN\Administrators
        /// </summary>
        public const string BUILTIN_Administrators = "S-1-5-32-544";

        /// <summary>
        /// BUILTIN\Users
        /// </summary>
        public const string BUILTIN_Users = "S-1-5-32-545";

        /// <summary>
        /// BUILTIN\Guests
        /// </summary>
        public const string BUILTIN_Guests = "S-1-5-32-546";

        /// <summary>
        /// BUILTIN\Power Users
        /// </summary>
        public const string BUILTIN_Power_Users = "S-1-5-32-547";

        /// <summary>
        /// BUILTIN\Backup Operators
        /// </summary>
        public const string BUILTIN_Backup_Operators = "S-1-5-32-551";

        /// <summary>
        /// BUILTIN\Replicator
        /// </summary>
        public const string BUILTIN_Replicator = "S-1-5-32-552";

        /// <summary>
        /// BUILTIN\Remote Desktop Users
        /// </summary>
        public const string BUILTIN_Remote_Desktop_Users = "S-1-5-32-555";

        /// <summary>
        /// BUILTIN\Network Configuration Operators
        /// </summary>
        public const string BUILTIN_Network_Configuration_Operators = "S-1-5-32-556";

        /// <summary>
        /// BUILTIN\Performance Monitor Users
        /// </summary>
        public const string BUILTIN_Performance_Monitor_Users = "S-1-5-32-558";

        /// <summary>
        /// BUILTIN\Performance Log Users
        /// </summary>
        public const string BUILTIN_Performance_Log_Users = "S-1-5-32-559";

        /// <summary>
        /// BUILTIN\Distributed COM Users
        /// </summary>
        public const string BUILTIN_Distributed_COM_Users = "S-1-5-32-562";

        /// <summary>
        /// BUILTIN\Cryptographic Operators
        /// </summary>
        public const string BUILTIN_Cryptographic_Operators = "S-1-5-32-569";

        /// <summary>
        /// BUILTIN\Event Log Readers
        /// </summary>
        public const string BUILTIN_Event_Log_Readers = "S-1-5-32-573";
        #endregion

        /// <summary>
        /// Gets a localized name of a given SID (Security Identifier) of a built in Windows account.
        /// </summary>
        /// <param name="SID">A SID (Security Identifier) as a string.</param>
        /// <returns>A localized name of a given SID (Security Identifier) of a built in Windows account. An empty string is returned in case of an error.</returns>
        public static string GetUserNameBySID(string SID)
        {
            try // try just in case..
            {
                // create a security identifier..
                System.Security.Principal.SecurityIdentifier sid =
                    new System.Security.Principal.SecurityIdentifier(SID);

                // "translate" the SID to an NTAccount class instance..
                System.Security.Principal.NTAccount account =
                    (System.Security.Principal.NTAccount)sid.Translate(typeof(System.Security.Principal.NTAccount));

                return account.Value; // return the localized name..
            }
            catch // return an empty string on an error..
            {
                return string.Empty;
            }
        }
    }
}
