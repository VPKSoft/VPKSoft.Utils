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
using System.Security.Cryptography;

namespace VPKSoft.Utils
{
    /// <summary>
    /// A class to protect and unprotect data using
    /// <para/>System.Security.Cryptography.ProtectedData class.
    /// </summary>
    public class BasicSecurity
    {
        /// <summary>
        /// Secures a byte array with a given entropy and DataProtectionScope.
        /// </summary>
        /// <param name="bytes">Bytes to secure.</param>
        /// <param name="entropy">An optional entropy to be used with the encryption.</param>
        /// <param name="scope">A System.Security.Cryptography.DataProtectionScope
        /// <para/>enumeration to be used with the encryption.</param>
        /// <returns>An encrypted byte array.</returns>
        public static byte[] Secure(byte[] bytes, byte[] entropy, DataProtectionScope scope)
        {            
            return ProtectedData.Protect(bytes, entropy, scope);
        }

        /// <summary>
        /// Secures a byte array with a given DataProtectionScope and null entropy.
        /// </summary>
        /// <param name="bytes">Bytes to secure.</param>
        /// <param name="scope">A System.Security.Cryptography.DataProtectionScope
        /// <para/>enumeration to be used with the encryption.</param>
        /// <returns>An encrypted byte array.</returns>
        public static byte[] Secure(byte[] bytes, DataProtectionScope scope)
        {
            return ProtectedData.Protect(bytes, null, scope);
        }

        /// <summary>
        /// Secures a byte array with DataProtectionScope.CurrentUser and null entropy.
        /// </summary>
        /// <param name="bytes">Bytes to secure.</param>
        /// <returns>An encrypted byte array.</returns>
        public static byte[] SecureCurrentUser(byte[] bytes)
        {
            return ProtectedData.Protect(bytes, null, DataProtectionScope.CurrentUser);
        }

        /// <summary>
        /// Secures a byte array with DataProtectionScope.CurrentUser and null entropy.
        /// </summary>
        /// <param name="bytes">Bytes to secure.</param>
        /// <returns>An encrypted byte array.</returns>
        public static byte[] Secure(byte[] bytes)
        {
            return ProtectedData.Protect(bytes, null, DataProtectionScope.CurrentUser);
        }

        /// <summary>
        /// Secures a byte array with DataProtectionScope.LocalMachine and null entropy.
        /// </summary>
        /// <param name="bytes">Bytes to secure.</param>
        /// <returns>An encrypted byte array.</returns>
        public static byte[] SecureLocalMachine(byte[] bytes)
        {
            return ProtectedData.Protect(bytes, null, DataProtectionScope.LocalMachine);
        }

        /// <summary>
        /// Secures a byte array with DataProtectionScope.CurrentUser and with a given entropy.
        /// </summary>
        /// <param name="bytes">Bytes to secure.</param>
        /// <param name="entropy">An entropy (byte array) to be used with the encryption.</param>
        /// <returns></returns>
        public static byte[] SecureCurrentUser(byte[] bytes, byte[] entropy)
        {
            return ProtectedData.Protect(bytes, entropy, DataProtectionScope.CurrentUser);
        }

        /// <summary>
        /// Secures a byte array with DataProtectionScope.CurrentUser and with a given entropy.
        /// </summary>
        /// <param name="bytes">Bytes to secure.</param>
        /// <param name="entropy">An entropy (byte array) to be used with the encryption.</param>
        /// <returns></returns>
        public static byte[] Secure(byte[] bytes, byte[] entropy)
        {
            return ProtectedData.Protect(bytes, entropy, DataProtectionScope.CurrentUser);
        }

        /// <summary>
        /// Secures a byte array with DataProtectionScope.LocalMachine and with a given entropy.
        /// </summary>
        /// <param name="bytes">Bytes to secure.</param>
        /// <param name="entropy">An entropy (byte array) to be used with the encryption.</param>
        /// <returns></returns>
        public static byte[] SecureLocalMachine(byte[] bytes, byte[] entropy)
        {
            return ProtectedData.Protect(bytes, entropy, DataProtectionScope.LocalMachine);
        }

        /// <summary>
        /// Decrypts a byte array with a given entropy and DataProtectionScope.
        /// </summary>
        /// <param name="bytes">Bytes to decrypt.</param>
        /// <param name="entropy">An optional entropy to be used with the decryption.</param>
        /// <param name="scope">A System.Security.Cryptography.DataProtectionScope
        /// <para/>enumeration to be used with the decryption.</param>
        /// <returns>An decrypted byte array.</returns>
        public static byte[] Unsecure(byte[] bytes, byte[] entropy, DataProtectionScope scope)
        {
            return ProtectedData.Unprotect(bytes, entropy, scope);
        }

        /// <summary>
        /// Decrypts a byte array with a DataProtectionScope and null entropy.
        /// </summary>
        /// <param name="bytes">Bytes to decrypt.</param>
        /// <param name="scope">A System.Security.Cryptography.DataProtectionScope
        /// <para/>enumeration to be used with the decryption.</param>
        /// <returns>An decrypted byte array.</returns>
        public static byte[] Unsecure(byte[] bytes, DataProtectionScope scope)
        {
            return ProtectedData.Protect(bytes, null, scope);
        }

        /// <summary>
        /// Decrypts a byte array with a DataProtectionScope.CurrentUser and null entropy.
        /// </summary>
        /// <param name="bytes">Bytes to decrypt.</param>
        /// <returns>An decrypted byte array.</returns>
        public static byte[] UnsecureCurrentUser(byte[] bytes)
        {
            return ProtectedData.Protect(bytes, null, DataProtectionScope.CurrentUser);
        }

        /// <summary>
        /// Decrypts a byte array with a DataProtectionScope.CurrentUser and null entropy.
        /// </summary>
        /// <param name="bytes">Bytes to decrypt.</param>
        /// <returns>An decrypted byte array.</returns>
        public static byte[] Unsecure(byte[] bytes)
        {
            return ProtectedData.Protect(bytes, null, DataProtectionScope.CurrentUser);
        }

        /// <summary>
        /// Decrypts a byte array with a DataProtectionScope.LocalMachine and null entropy.
        /// </summary>
        /// <param name="bytes">Bytes to decrypt.</param>
        /// <returns>An decrypted byte array.</returns>
        public static byte[] UnsecureLocalMachine(byte[] bytes)
        {
            return ProtectedData.Protect(bytes, null, DataProtectionScope.LocalMachine);
        }

        /// <summary>
        /// Decrypts a byte array with a given entropy and DataProtectionScope.CurrentUser.
        /// </summary>
        /// <param name="bytes">Bytes to decrypt.</param>
        /// <param name="entropy">An optional entropy to be used with the decryption.</param>
        /// <returns>An decrypted byte array.</returns>
        public static byte[] UnsecureCurrentUser(byte[] bytes, byte[] entropy)
        {
            return ProtectedData.Protect(bytes, entropy, DataProtectionScope.CurrentUser);
        }

        /// <summary>
        /// Decrypts a byte array with a given entropy and DataProtectionScope.CurrentUser.
        /// </summary>
        /// <param name="bytes">Bytes to decrypt.</param>
        /// <param name="entropy">An optional entropy to be used with the decryption.</param>
        /// <returns>An decrypted byte array.</returns>
        public static byte[] Unsecure(byte[] bytes, byte[] entropy)
        {
            return ProtectedData.Protect(bytes, entropy, DataProtectionScope.CurrentUser);
        }


        /// <summary>
        /// Decrypts a byte array with a given entropy and DataProtectionScope.LocalMachine.
        /// </summary>
        /// <param name="bytes">Bytes to decrypt.</param>
        /// <param name="entropy">An optional entropy to be used with the decryption.</param>
        /// <returns>An decrypted byte array.</returns>
        public static byte[] UnsecureLocalMachine(byte[] bytes, byte[] entropy)
        {
            return ProtectedData.Protect(bytes, entropy, DataProtectionScope.LocalMachine);
        }
    }
}
