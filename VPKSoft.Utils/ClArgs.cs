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

namespace VPKSoft.Utils
{
    /// <summary>
    /// A class for handling program command-line arguments.
    /// </summary>
    public class ProgramArgumentCollection
    {
        /// <summary>
        /// A constructor that handles the program's command-line arguments.
        /// <para/>The arguments are splitted from a position of an equals
        /// <para/>character (=) and the first part becomes the argument name and
        /// <para/>the second part becomes the arguments value. 
        /// <para/>If there is no splitting character the value of an argument
        /// <para/>simply becomes "1".
        /// </summary>
        public ProgramArgumentCollection()
        {
            foreach (string arg in System.Environment.GetCommandLineArgs())
            {
                try
                {
                    KeyValuePair<string, string> a = new KeyValuePair<string, string>(arg.Split('=')[0], arg.Split('=')[1]);
                    args.Add(a);
                }
                catch
                {
                    KeyValuePair<string, string> a = new KeyValuePair<string, string>(arg, "1");
                    args.Add(a);
                }
            }
        }

        /// <summary>
        /// Should the class instance ignore the case of an argument.
        /// <para/>The value of an argument is not affected.
        /// </summary>
        public bool IgnoreCase = false;

        /// <summary>
        /// Should the class instance preceeding hyphens
        /// <para/>of an argument (e.g. --lang=1033 --> lang=1033).
        /// <para/>The value of an argument is not affected.
        /// </summary>
        public bool IgnorePreceedingHyphens = false;

        /// <summary>
        /// Private list of arguments.
        /// </summary>
        private List<KeyValuePair<string, string>> args = new List<KeyValuePair<string, string>>();

        /// <summary>
        /// Get's a value of an argument with a given name.
        /// </summary>
        /// <param name="arg">The argument name which value to get.</param>
        /// <returns>The value of an argument if such exists, otherwise null is returned.</returns>
        public string this[string arg]
        {
            get
            {
                string keyManipulated;
                foreach (KeyValuePair<string, string> a in args)
                {
                    keyManipulated = a.Key;
                    if (IgnoreCase)
                    {
                        keyManipulated = keyManipulated.ToUpper();
                    }

                    if (IgnorePreceedingHyphens)
                    {
                        keyManipulated = keyManipulated.TrimStart('-');
                    }

                    if ((!IgnoreCase && keyManipulated == arg) ||
                        (IgnoreCase && keyManipulated == arg.ToUpper()))
                    {
                        return a.Value;
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// Get's a value if an argument with a given name exists.
        /// </summary>
        /// <param name="arg">The argument name of which existance to check.</param>
        /// <returns>True if the argument with a given name exists, otherwise false.</returns>
        public bool ArgumentExists(string arg)
        {
            return this[arg] != null;
        }
    }
}
