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
        /// Counts the occurrences of a char in the string.
        /// </summary>
        /// <param name="str">A string instance.</param>
        /// <param name="occurence">The character of which occurrences to count.</param>
        /// <returns>A number of occurrences in the string.</returns>
        public static int Count(this string str, char occurence)
        {
            return str.Count(f => f == occurence);
        }

        /// <summary>
        /// Counts the occurrences of a string in the string.
        /// </summary>
        /// <param name="str">A string instance.</param>
        /// <param name="occurrence">The string if which occurrences to count.</param>
        /// <returns>A number of occurrences in the string.</returns>
        public static int Count(this string str, string occurrence)
        {
            return str.Split(new[] { occurrence }, StringSplitOptions.None).Length;
        }

        /// <summary>
        /// Gets the positions of a character occurrences in a string.
        /// </summary>
        /// <param name="str">A string instance.</param>
        /// <param name="occurence">The character of which positions to locate in the string.</param>
        /// <returns>A list of positions the occurence was in the string.</returns>
        public static List<int> Pos(this string str, char occurence)
        {
            List<int> result = new List<int>();

            int index = 0;
            while ((index = str.IndexOf(occurence, index)) != -1)
            {
                result.Add(index);
                index++;
            }
            
            return result;
        }

        /// <summary>
        /// Gets the positions of a string occurrences in a string.
        /// </summary>
        /// <param name="str">A string instance.</param>
        /// <param name="occurence">The string of which positions to locate in the string.</param>
        /// <returns>A list of positions the occurence was in the string.</returns>
        public static List<int> Pos(this string str, string occurence)
        {
            List<int> result = new List<int>();

            int index = 0;
            while ((index = str.IndexOf(occurence, index, StringComparison.Ordinal)) != -1)
            {
                result.Add(index);
                index++;
            }

            return result;
        }

        /// <summary>
        /// Gets a value whether the string matches the given object array string representation. I.e SQL IN.
        /// </summary>
        /// <param name="str">A string instance.</param>
        /// <param name="values">The values to check for equality with the string instance.</param>
        /// <returns><c>true</c> if the string matches one of the given object string representations, <c>false</c> otherwise.</returns>
        public static bool In(this string str, params object[] values)
        {
            return In(str, StringComparison.Ordinal, values);
        }

        /// <summary>
        /// Gets a value whether the string matches the given object array string representation. I.e SQL IN.
        /// </summary>
        /// <param name="str">A string instance.</param>
        /// <param name="comparisonType">One of the enumeration values that specifies the rules for the comparison.</param>
        /// <param name="values">The values to check for equality with the string instance.</param>
        /// <returns><c>true</c> if the string matches one of the given object string representations, <c>false</c> otherwise.</returns>
        public static bool In(this string str, StringComparison comparisonType, params object [] values)
        {
            if (values.Length == 0)
            {
                return false;
            }

            foreach (var value in values)
            {
                if (value == null)
                {
                    continue;
                }

                if (string.Equals(str, value.ToString(), comparisonType))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Gets a value whether the string part matches the given object array string representation. I.e SQL IN.
        /// </summary>
        /// <param name="str">A string instance.</param>
        /// <param name="comparisonType">One of the enumeration values that specifies the rules for the comparison.</param>
        /// <param name="values">The values to check for partly equality with the string instance.</param>
        /// <returns><c>true</c> if the part of the string matches one of the given object string representations, <c>false</c> otherwise.</returns>
        public static bool InPart(this string str, StringComparison comparisonType, params object[] values)
        {
            if (values.Length == 0)
            {
                return false;
            }

            foreach (var value in values)
            {
                if (value == null)
                {
                    continue;
                }

                if (str.IndexOf(value.ToString(), comparisonType) != -1)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Gets a value whether the string part matches the given object array string representation. I.e SQL IN.
        /// </summary>
        /// <param name="str">A string instance.</param>
        /// <param name="values">The values to check for partly equality with the string instance.</param>
        /// <returns><c>true</c> if the part of the string matches one of the given object string representations, <c>false</c> otherwise.</returns>
        public static bool InPart(this string str, params object[] values)
        {
            return InPart(str, StringComparison.Ordinal, values);
        }

    }
}
