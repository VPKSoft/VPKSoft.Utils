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
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace VPKSoft.Utils
{
    /// <summary>
    /// Utilities for MD5 hash calculations.
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public class IoHash
    {
        /// <summary>
        /// Appends (TransformBlock) a stream to a MD5 instance.
        /// </summary>
        /// <param name="stream">The stream to append.</param>
        /// <param name="md5">A reference to a MD5 instance to append to.</param>
        // ReSharper disable once InconsistentNaming
        public static void MD5AppendStream(Stream stream, ref MD5 md5)
        {
            byte [] buffer = new byte[1000000]; // go with 1 MB
            stream.Position = 0;
            int pos = 0, count;

            while ((count = stream.Read(buffer, pos, 1000000)) > 0)
            {
                md5.TransformBlock(buffer, 0, count, buffer, 0);
            }
        }

        /// <summary>
        /// Appends (TransformBlock) a variable array of bytes to a MD5 instance.
        /// </summary>
        /// <param name="bytes">A variable array of bytes to append.</param>
        /// <param name="md5">A reference to a MD5 instance to append to.</param>
        // ReSharper disable once InconsistentNaming
        // ReSharper disable once UnusedMember.Global
        public static void MD5AppendBytes(byte [] bytes, ref MD5 md5)
        {
            md5.TransformBlock(bytes, 0, bytes.Length, bytes, 0);
        }

        /// <summary>
        /// Appends (TransformBlock) a file to an MD5 instance.
        /// </summary>
        /// <param name="fileName">The name of the file to append.</param>
        /// <param name="md5">A reference to a MD5 instance to append to.</param>
        // ReSharper disable once InconsistentNaming
        public static void MD5AppendFile(string fileName, ref MD5 md5)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                MD5AppendStream(fs, ref md5);
            }
        }

        /// <summary>
        /// Appends (TransformBlock) a string in Unicode encoding to a MD5 instance.
        /// </summary>
        /// <param name="stringToAppend">String to append.</param>
        /// <param name="md5">A reference to a MD5 instance to append to.</param>
        // ReSharper disable once InconsistentNaming
        public static void MD5AppendString(string stringToAppend, ref MD5 md5)
        {
            byte[] buffer = Encoding.Unicode.GetBytes(stringToAppend.ToCharArray());
            md5.TransformBlock(buffer, 0, buffer.Length, buffer, 0);
        }

        /// <summary>
        /// Appends (TransformBlock) a string in a given <paramref name="encoding"/> encoding to a MD5 instance.
        /// </summary>
        /// <param name="stringToAppend">String to append.</param>
        /// <param name="md5">A reference to a MD5 instance to append to.</param>
        /// <param name="encoding">The encoding to append the string.</param>
        // ReSharper disable once InconsistentNaming
        // ReSharper disable once UnusedMember.Global
        public static void MD5AppendString(string stringToAppend, ref MD5 md5, Encoding encoding)
        {
            byte[] buffer = Encoding.Unicode.GetBytes(stringToAppend.ToCharArray());
            md5.TransformBlock(buffer, 0, buffer.Length, buffer, 0);
        }

        /// <summary>
        /// Finalizes (TransformFinalBlock) a MD5 instance with a zero-sized buffer.
        /// </summary>
        /// <param name="md5">A reference to a MD5 instance to finalize.</param>
        // ReSharper disable once InconsistentNaming
        public static void MD5FinalizeBlock(ref MD5 md5)
        {
            md5.TransformFinalBlock(new byte[0], 0, 0);
        }

        /// <summary>
        /// Finalizes (TransformFinalBlock) a MD5 instance with a zero-sized buffer
        /// and converts the hash into a hexadecimal string representation.
        /// </summary>
        /// <param name="md5">A reference to a MD5 instance to finalize.</param>
        /// <returns>A hexadecimal string representation of the MD5 hash.</returns>
        // ReSharper disable once InconsistentNaming
        public static string MD5GetHashString(ref MD5 md5)
        {
            MD5FinalizeBlock(ref md5);
            return "0x" + BitConverter.ToString(md5.Hash, 0).Replace("-", "");
        }

        /// <summary>
        /// Enumerates an entire directory recursively and calculates a MD5
        /// hash of its files, file names and directory names.
        /// </summary>
        /// <param name="directoryEnumerate">A directory to enumerate.</param>
        /// <param name="ignoreUnreadableFiles">If true causes the method to ignore locked files.</param>
        /// <returns>
        /// A hexadecimal string representation of the MD5 hash
        /// or an empty string if the operation failed.
        /// </returns>
        // ReSharper disable once InconsistentNaming
        // ReSharper disable once UnusedMember.Global
        public static string MD5HashDirSimple(string directoryEnumerate, bool ignoreUnreadableFiles = false)
        {
            try
            {
                MD5 md5 = MD5.Create();
                string[] fileArray = Directory
                    .GetFileSystemEntries(directoryEnumerate + "\\", "*.*", SearchOption.AllDirectories).ToArray();
                List<string> fileNames = new List<string>();
                fileNames.AddRange(fileArray);
                fileNames.Sort();
                List<string> directories = new List<string>();
                // ReSharper disable once TooWideLocalVariableScope
                string directory;
                foreach (string fileName in fileNames)
                {
                    directory = Directory.Exists(fileName)
                        ? fileName.Replace(directoryEnumerate, string.Empty).TrimStart('\\')
                        : Path.GetDirectoryName(fileName)?.Replace(directoryEnumerate, string.Empty).TrimStart('\\');

                    if (Directory.Exists(fileName) && directory != string.Empty)
                    {
                        if (!directories.Contains(directory))
                        {
                            MD5AppendString(directory, ref md5);
                            directories.Add(directory);
                        }
                    }
                    else
                    {
                        if (!directories.Contains(directory))
                        {
                            MD5AppendString(directory, ref md5);
                            directories.Add(directory);
                        }
                        MD5AppendString(fileName, ref md5);
                        if (ignoreUnreadableFiles)
                        {
                            try
                            {
                                MD5AppendFile(fileName, ref md5);
                            }
                            catch
                            {
                                // ignored..
                            }
                        }
                        else
                        {
                            MD5AppendFile(fileName, ref md5);
                        }
                    }
                }
                return MD5GetHashString(ref md5);
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
