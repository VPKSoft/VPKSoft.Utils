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
    /// Utilities for converting bytes from a 
    /// <para/>hexadecimal string representation
    /// <para/>and vice versa.
    /// </summary>
    public class Bytes
    {
        /// <summary>
        /// Converts a hexadecimal string to a a byte array.
        /// </summary>
        /// <param name="hex">A hexadecimal string to convert.</param>
        /// <returns>A byte array which has been converted from a hexadecimal string.</returns>
        public static byte[] StringToByteArray(String hex)
        {
            hex = hex.Replace("0x", string.Empty);
            int nChars = hex.Length;
            byte[] bytes = new byte[nChars / 2];
            for (int i = 0; i < nChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }
            return bytes;
        }

        /// <summary>
        /// Converts an array of bytes into a hexadecimal
        /// <para/>string presentation.
        /// </summary>
        /// <param name="value">A byte array.</param>
        /// <returns>A hexadecimal string presentation</returns>
        public static string BytesToHexString(byte[] value)
        {
            string hex = BitConverter.ToString(value);
            return "0x" + hex.Replace("-", "");
        }


        /// <summary>
        /// Converts a specified string into a hexadecimal presentation
        /// <para/>of the bytes in a given string.
        /// </summary>
        /// <param name="value">A string to convert into hexadecimal presentation of the bytes in it.</param>
        /// <param name="enc">An instance of System.Text.Encoding class
        /// <para/>to be used for the hexadecimal presentation conversion.</param>
        /// <returns>A hexadecimal string presentation of the given value with a given encoding.</returns>
        public static string StringToHexString(string value, Encoding enc)
        {
            byte[] bData = enc.GetBytes(value.ToCharArray());
            return BytesToHexString(bData);
        }

        /// <summary>
        /// Converts a specified string into a hexadecimal presentation
        /// <para/>of the bytes in a given string.
        /// </summary>
        /// <param name="value">A string to convert into hexadecimal presentation of the bytes in it.</param>
        /// <returns>A hexadecimal string presentation of the given value with an UTF8 encoding.</returns>
        public static string StringToHexStringUTF8(string value)
        {
            return StringToHexString(value, Encoding.UTF8);
        }

        /// <summary>
        /// Converts a specified string into a hexadecimal presentation
        /// <para/>of the bytes in a given string.
        /// </summary>
        /// <param name="value">A string to convert into hexadecimal presentation of the bytes in it.</param>
        /// <returns>A hexadecimal string presentation of the given value with an UTF32 encoding.</returns>
        public static string StringToHexStringUTF32(string value)
        {
            return StringToHexString(value, Encoding.UTF32);
        }

        /// <summary>
        /// Converts a specified string into a hexadecimal presentation
        /// <para/>of the bytes in a given string.
        /// </summary>
        /// <param name="value">A string to convert into hexadecimal presentation of the bytes in it.</param>
        /// <returns>A hexadecimal string presentation of the given value with a UTF8 encoding.</returns>
        public static string StringToHexStringUnicode(string value)
        {
            return StringToHexString(value, Encoding.Unicode);
        }

        /// <summary>
        /// Converts a specified string into a hexadecimal presentation
        /// <para/>of the bytes in a given string.
        /// </summary>
        /// <param name="value">A string to convert into hexadecimal presentation of the bytes in it.</param>
        /// <returns>A hexadecimal string presentation of the given value with an ASCII encoding.</returns>
        public static string StringToHexStringASCII(string value)
        {
            return StringToHexString(value, Encoding.ASCII);
        }

        /// <summary>
        /// Converts a specified string into a hexadecimal presentation
        /// <para/>of the bytes in a given string.
        /// </summary>
        /// <param name="value">A string to convert into hexadecimal presentation of the bytes in it.</param>
        /// <returns>A hexadecimal string presentation of the given value with a BigEndianUnicode encoding.</returns>
        public static string StringToHexStringBigEndianUnicode(string value)
        {
            return StringToHexString(value, Encoding.BigEndianUnicode);
        }

        /// <summary>
        /// Converts a specified string into a hexadecimal presentation
        /// <para/>of the bytes in a given string.
        /// </summary>
        /// <param name="value">A string to convert into hexadecimal presentation of the bytes in it.</param>
        /// <returns>A hexadecimal string presentation of the given value with an UTF7 encoding.</returns>
        public static string StringToHexStringUTF7(string value)
        {
            return StringToHexString(value, Encoding.UTF7);
        }

        /// <summary>
        /// Converts a byte array to string with a given encoding.
        /// </summary>
        /// <param name="bArr">A byte array to be converted into a string.</param>
        /// <param name="enc">An encoding to use to convert the byte array to a string.</param>
        /// <returns>A string converted from the byte array with the given encoding.</returns>
        public static string ByteArrayToString(byte[] bArr, Encoding enc)
        {
            return enc.GetString(bArr);
        }

        /// <summary>
        /// Converts a byte array to string with UTF8 encoding.
        /// </summary>
        /// <param name="bArr">A byte array to be converted into a string.</param>
        /// <returns>A string converted from the byte array with UTF8 encoding.</returns>
        public static string ByteArrayToStringUTF8(byte[] bArr)
        {
            return ByteArrayToString(bArr, Encoding.UTF8);
        }

        /// <summary>
        /// Converts a byte array to string with Unicode encoding.
        /// </summary>
        /// <param name="bArr">A byte array to be converted into a string.</param>
        /// <returns>A string converted from the byte array with Unicode encoding.</returns>
        public static string ByteArrayToStringUnicode(byte[] bArr)
        {
            return ByteArrayToString(bArr, Encoding.Unicode);
        }

        /// <summary>
        /// Converts a byte array to string with ASCII encoding.
        /// </summary>
        /// <param name="bArr">A byte array to be converted into a string.</param>
        /// <returns>A string converted from the byte array with ASCII encoding.</returns>
        public static string ByteArrayToStringASCII(byte[] bArr)
        {
            return ByteArrayToString(bArr, Encoding.ASCII);
        }

        /// <summary>
        /// Converts a byte array to string with BigEndianUnicode encoding.
        /// </summary>
        /// <param name="bArr">A byte array to be converted into a string.</param>
        /// <returns>A string converted from the byte array with BigEndianUnicode encoding.</returns>
        public static string ByteArrayToStringBigEndianUnicode(byte[] bArr)
        {
            return ByteArrayToString(bArr, Encoding.BigEndianUnicode);
        }

        /// <summary>
        /// Converts a byte array to string with UTF32 encoding.
        /// </summary>
        /// <param name="bArr">A byte array to be converted into a string.</param>
        /// <returns>A string converted from the byte array with UTF32 encoding.</returns>
        public static string ByteArrayToStringUTF32(byte[] bArr)
        {
            return ByteArrayToString(bArr, Encoding.UTF32);
        }

        /// <summary>
        /// Converts a byte array to string with UTF7 encoding.
        /// </summary>
        /// <param name="bArr">A byte array to be converted into a string.</param>
        /// <returns>A string converted from the byte array with UTF7 encoding.</returns>
        public static string ByteArrayToStringUTF7(byte[] bArr)
        {
            return ByteArrayToString(bArr, Encoding.UTF7);
        }
    }
}
