#region License
/*
VPKSoft.Utils

Some utilities by VPKSoft.
Copyright (C) 2015 VPKSoft, Petteri Kautonen

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
    /// Extensions to string class.
    /// </summary>
    public static class Strings
    {
        /// <summary>
        /// Trims a given amount of characters from a string's end.
        /// </summary>
        /// <param name="str">A string to trim.</param>
        /// <param name="trimChar">A character to remove (trim).</param>
        /// <param name="count">A number which indicates how many
        /// <para/>characters to remove (trim) from a string.</param>
        /// <returns>A string with trimmed from the end.</returns>
        public static string TrimEndCount(this string str, char trimChar, int count = 1)
        {
            List<char> usedTrimChars = new List<char>();
            while (count > 0 && !string.IsNullOrEmpty(str))
            {
                if (str[str.Length - 1] == trimChar)
                {
                    str = str.Substring(0, str.Length - 1);
                }
                count--;
            }
            return str;
        }

        /// <summary>
        /// Trims a given amount of characters from a string's start.
        /// </summary>
        /// <param name="str">A string to trim.</param>
        /// <param name="trimChar">A character to remove (trim).</param>
        /// <param name="count">A number which indicates how many
        /// <para/>characters to remove (trim) from a string.</param>
        /// <returns>A string with trimmed from the start.</returns>
        public static string TrimStartCount(this string str, char trimChar, int count = 1)
        {
            List<char> usedTrimChars = new List<char>();
            while (count > 0 && !string.IsNullOrEmpty(str))
            {
                if (str[0] == trimChar)
                {
                    str = str.Substring(1);
                }
                count--;
            }
            return str;
        }

        /// <summary>
        /// Counts the occurences of a char in the string.
        /// </summary>
        /// <param name="str">A string instance.</param>
        /// <param name="occurence">The character of which occurences to count.</param>
        /// <returns>A number of occurences in the string.</returns>
        public static int Count(this string str, char occurence)
        {
            return str.Count(f => f == occurence);
        }

        /// <summary>
        /// Counts the occurences of a string in the string.
        /// </summary>
        /// <param name="str">A string instance.</param>
        /// <param name="occurence">The string if which occurences to count.</param>
        /// <returns>A number of occurences in the string.</returns>
        public static int Count(this string str, string occurence)
        {
            return str.Split(new string[] { occurence }, StringSplitOptions.None).Length;
        }

        /// <summary>
        /// Gets the positions of a character occurences in a string.
        /// </summary>
        /// <param name="str">A string instance.</param>
        /// <param name="occurence">The character of which positions to locate in the string.</param>
        /// <returns>A list of positions the occurence was in the string.</returns>
        public static List<int> Pos(this string str, char occurence)
        {
            List<int> retval = new List<int>();

            int index = 0;
            while ((index = str.IndexOf(occurence, index)) != -1)
            {
                retval.Add(index);
                index++;
            }
            
            return retval;
        }

        /// <summary>
        /// Gets the positions of a string occurences in a string.
        /// </summary>
        /// <param name="str">A string instance.</param>
        /// <param name="occurence">The string of which positions to locate in the string.</param>
        /// <returns>A list of positions the occurence was in the string.</returns>
        public static List<int> Pos(this string str, string occurence)
        {
            List<int> retval = new List<int>();

            int index = 0;
            while ((index = str.IndexOf(occurence, index)) != -1)
            {
                retval.Add(index);
                index++;
            }

            return retval;
        }

    }
}
