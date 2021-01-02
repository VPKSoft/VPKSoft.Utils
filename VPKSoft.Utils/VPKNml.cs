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
using System.IO;

namespace VPKSoft.Utils
{
    /// <summary>
    /// A class for handling VPKSoft NOT markup language files = (VNml (*.vnml))).
    /// </summary>
    public class VPKNml
    {
        /// <summary>
        /// An exception is thrown if the first line is not one of the(se) strings.
        /// <para/>As versions grow, more will appear.
        /// </summary>
        const string VersioStr10 = "-- VNml v.1.0. --";

        /// <summary>
        /// An array of acceptable version strings.
        /// </summary>
        string[] VersioStr = { VersioStr10 };


        /// <summary>
        /// An insternal class to hold a single section of the file.
        /// </summary>
        private class VmlEntry
        {
            /// <summary>
            /// The section name.
            /// </summary>
            public string Name;

            /// <summary>
            /// Main level comments.
            /// </summary>
            public List<string> Comments = new List<string>();

            /// <summary>
            /// The section values.
            /// </summary>
            public List<KeyValuePair<string, VmlSubEntry>> Values = new List<KeyValuePair<string, VmlSubEntry>>();

            /// <summary>
            /// Constructor for the VmlEntry class.
            /// </summary>
            /// <param name="name">The section name.</param>
            public VmlEntry(string name)
            {
                Name = name;
            }
        }

        /// <summary>
        /// An internal class to hold a single value of a single section of the file.
        /// </summary>
        private class VmlSubEntry
        {
            /// <summary>
            /// The value. 
            /// </summary>
            public object Value;

            /// <summary>
            /// Comments givent to the value.
            /// </summary>
            public List<string> Comments;

            /// <summary>
            /// The constructor for the internal class to hold
            /// <para/>a single value of a single section of the file.
            /// </summary>
            /// <param name="value"></param>
            /// <param name="comments"></param>
            public VmlSubEntry(object value, List<string> comments)
            {
                this.Value = value;
                this.Comments = comments == null ? new List<string>() : comments;
            }
        }

        /// <summary>
        /// A namespace to use.
        /// </summary>
        private string nameSpace = string.Empty;

        /// <summary>
        /// A namespace to use.
        /// <remarks>White spaces in the name will be removed.</remarks>
        /// </summary>
        public string NameSpace
        {
            get
            {
                return nameSpace == string.Empty ? nameSpace : nameSpace + ":";
            }

            set
            {
                nameSpace = value.Trim().Replace(" ", string.Empty);
            }
        }

        /// <summary>
        /// Internal list for holding file section.
        /// </summary>
        private List<VmlEntry> fileContents = new List<VmlEntry>();

        /// <summary>
        /// A constructor with no namespace to use.
        /// </summary>
        public VPKNml()
        {

        }

        /// <summary>
        /// Constructor with a namespace.
        /// </summary>
        /// <param name="nameSpace">A namespace to use.</param>
        public VPKNml(string nameSpace)
        {
            NameSpace = nameSpace;
        }

        /// <summary>
        /// Loads a vnml document.
        /// </summary>
        /// <param name="fileName">A file from which the vnml document should be loaded from.</param>
        public void Load(string fileName)
        {
            if (File.Exists(fileName))
            {
                List<string> lines = new List<string>();
                lines.AddRange(File.ReadAllLines(fileName, Encoding.UTF8));

                bool versionFound = false;
                if (lines.Count > 0)
                {
                    foreach (string v in VersioStr)
                    {
                        if (lines[0] == v)
                        {
                            versionFound = true;
                        }
                    }
                    if (!versionFound)
                    {
                        throw new InvalidDataException("Badly formatted VNml.");
                    }
                }

                if (lines.Count > 0)
                {
                    lines.RemoveAt(0);
                }

                string currentCat = string.Empty;
                List<string> commentCache = new List<string>();
                foreach (string str in lines)
                {
                    if (str.Trim(' ', '\t').StartsWith(";")) // ignore comment but save it
                    {
                        commentCache.Add(str.TrimStart(' ', '\t').TrimStartCount(';'));
                        continue;
                    }

                    if (str.Trim(' ', '\t') == string.Empty) // ignore empty line
                    {
                        continue;
                    }

                    if (!str.StartsWith("\t"))
                    {
                        if (!fileContents.Exists((e) => e.Name == str))
                        {
                            VmlEntry addEntry = new VmlEntry(str);
                            addEntry.Comments.AddRange(commentCache);
                            commentCache.Clear();
                            fileContents.Add(addEntry);

                            currentCat = str;
                        }
                        else
                        {
                            throw new InvalidDataException("Badly formatted VNml.");
                        }
                    }
                    else if (str.StartsWith("\t"))
                    {
                        string valueStr = str;
                        valueStr = valueStr.TrimStart('\t');
                        valueStr.IndexOf('=');

                        string valueStr1 = valueStr.Substring(0, valueStr.IndexOf('='));
                        string valueStr2 = valueStr.Substring(valueStr.IndexOf('=') + 1);
                        valueStr2 = valueStr2.TrimStartCount('[').TrimEndCount(']');

                        fileContents.Single((e) => e.Name == currentCat).Values.Add(new KeyValuePair<string, VmlSubEntry>(valueStr1, new VmlSubEntry(valueStr2, new List<string>(commentCache))));
                        commentCache.Clear();
                    }
                }
            }
        }

        /// <summary>
        /// Deletes sections matching a given mask from the current namespace.
        /// </summary>
        /// <param name="mask">A string indicating sections to be deleted.
        /// <para/>If an asterisk (*) mark is the last character, all sections
        /// <para/>starting with the given mask are deleted.
        /// <para/>If the give mask is an asterisk (*) character, all the contents will be deleted.</param>
        public void DeleteSections(string mask)
        {
            if (mask == "*")
            {
                fileContents.Clear();
                return;
            }

            bool isAsteriskMask = mask.EndsWith("*");
            mask = mask.TrimEndCount('*');
            mask = NameSpace + mask;

            if (mask == string.Empty)
            {
                return;
            }

            for (int i = fileContents.Count - 1; i >= 0; i--)
            {
                if (isAsteriskMask)
                {
                    if (fileContents[i].Name.StartsWith(mask))
                    {
                        fileContents.RemoveAt(i);
                    }
                }
                else
                {
                    if (fileContents[i].Name == mask)
                    {
                        fileContents.RemoveAt(i);

                    }
                }
            }
        }

        /// <summary>
        /// Deletes values from a given section matching a given mask from the current namespace.
        /// </summary>
        /// <param name="section">A section to delete the values from in the current namespace.</param>
        /// <param name="mask">A string indicating values to be deleted.
        /// <para/>If an asterisk (*) mark is the last character, all values
        /// <para/>starting with the given mask are deleted.
        /// <para/>If the section has no values after the operation the section is also deleted.</param>
        public void DeleteValues(string section, string mask)
        {
            bool isAsteriskMask = mask.EndsWith("*");
            mask = mask.TrimEndCount('*');

            if (mask == string.Empty)
            {
                return;
            }

            section = NameSpace + section;


            for (int i = fileContents.Count - 1; i >= 0; i--)
            {
                if (fileContents[i].Name == section)
                {
                    for (int j = fileContents[i].Values.Count - 1; j >= 0; j++)
                    {
                        if (isAsteriskMask)
                        {
                            if (fileContents[i].Values[j].Key.StartsWith(mask))
                            {
                                fileContents[i].Values.RemoveAt(j);
                            }
                        }
                        else
                        {
                            if (fileContents[i].Values[j].Key == mask)
                            {
                                fileContents[i].Values.RemoveAt(j);
                            }
                        }
                    }

                    if (fileContents[i].Values.Count == 0)
                    {
                        fileContents.RemoveAt(i);
                    }
                }
            }
        }


        /// <summary>
        /// Sets a comment to a to a given section or to the section's value name. 
        /// </summary>
        /// <param name="name">A section name.</param>
        /// <param name="valueName">A value name or null if the comment is to be given to a section.</param>
        /// <param name="comment">A comment to set.</param>
        /// <returns>True if the comment was successfully given, otherwise false.</returns>
        public bool SetComment(string name, string valueName, params string[] comment)
        {
            return SetComment(new List<string>(comment), name, valueName);
        }

        /// <summary>
        /// Sets a comment to a to a given section or to the section's value name. 
        /// </summary>
        /// <param name="comment">A comment to set.</param>
        /// <param name="name">A section name.</param>
        /// <param name="valueName">A value name or null if the comment is to be given to a section.</param>
        /// <returns>True if the comment was successfully given, otherwise false.</returns>
        public bool SetComment(List<string> comment, string name, string valueName)
        {
            name = NameSpace + name;
            try
            {
                if (valueName == null)
                {
                    fileContents.Single((e) => e.Name == name).Comments.Clear();
                    fileContents.Single((e) => e.Name == name).Comments.AddRange(comment);
                    return true;
                }
                else
                {
                    VmlEntry entry = fileContents.Single((e) => e.Name == name);
                    for (int i = entry.Values.Count - 1; i >= 0; i--)
                    {
                        if (entry.Values[i].Key == valueName)
                        {
                            entry.Values[i].Value.Comments.Clear();
                            entry.Values[i].Value.Comments.AddRange(comment);
                            return true;
                        }
                    }

                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a comment of a given section or a comment of the section's value name. 
        /// </summary>
        /// <param name="name">A section name.</param>
        /// <param name="valueName">A value name or null if the section's comment is needed.</param>
        /// <returns>A comment of a given section or a comment of the section's value name.
        /// <para/>If a value or a section is not found, null is returned.</returns>
        public List<string> GetComment(string name, string valueName)
        {
            name = NameSpace + name;
            try
            {
                if (valueName == null)
                {
                    return fileContents.Single((e) => e.Name == name).Comments;
                }
                else
                {
                    VmlEntry entry = fileContents.Single((e) => e.Name == name);
                    for (int i = entry.Values.Count - 1; i >= 0; i--)
                    {
                        if (entry.Values[i].Key == valueName)
                        {
                            return entry.Values[i].Value.Comments.Count == 0 ? null : entry.Values[i].Value.Comments;
                        }
                    }

                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Gets an object with a given name and it's value name.
        /// </summary>
        /// <param name="name">A name of the object.</param>
        /// <param name="valueName">A value name of the object</param>
        /// <param name="defaultValue">A default value to return if the object was not found.</param>
        /// <returns>The object in string presentation or
        /// <para/>the default value if object with name and value name does not exist.</returns>
        /// <remarks>If the value name starts with "BIN:" the value will
        /// <para/>be returned as an array of bytes.</remarks>
        public object this[string name, string valueName, object defaultValue]
        {
            get
            {
                return this[name, valueName] == null ? defaultValue : this[name, valueName];
            }
        }


        /// <summary>
        /// Gets or sets an object with a given name and it's value name.
        /// </summary>
        /// <param name="name">A name of the object.</param>
        /// <param name="valueName">A value name of the object</param>
        /// <returns>The object in string presentation or
        /// <para/>null if object with name and value name does not exist.</returns>
        /// <remarks>If the value name starts with "BIN:" the value will
        /// <para/>be saved as hexadecimal string presentation
        /// <para/>and returned as an array of bytes.</remarks>
        public object this[string name, string valueName]
        {
            get
            {
                name = NameSpace + name;
                try
                {
                    if (valueName.StartsWith("BIN:"))
                    {
                        if (fileContents.Single((e) => e.Name == name).Values.Single((s) => s.Key == valueName).Value.Value.ToString() == "0x")
                        {
                            return null;
                        }
                        return Bytes.StringToByteArray(fileContents.Single((e) => e.Name == name).Values.Single((s) => s.Key == valueName).Value.Value.ToString());
                    }
                    else
                    {
                        return fileContents.Single((e) => e.Name == name).Values.Single((s) => s.Key == valueName).Value.Value;
                    }
                }
                catch
                {
                    return null;
                }
            }

            set
            {
                name = NameSpace + name;
                List<string> saveComments = new List<string>();
                if (fileContents.Exists((e) => e.Name == name))
                {
                    try
                    {
                        VmlEntry entry = fileContents.Single((e) => e.Name == name);
                        for (int i = entry.Values.Count - 1; i >= 0; i--)
                        {
                            if (entry.Values[i].Key == valueName)
                            {
                                saveComments.AddRange(entry.Values[i].Value.Comments);
                                entry.Values.RemoveAt(i);
                            }
                        }
                        if (value == null)
                        {
                            return;
                        }
                    }
                    catch
                    {
                        throw new InvalidOperationException("VNml parse error.");
                    }
                }

                if (value != null)
                {
                    VmlEntry entry;
                    try
                    {
                        entry = fileContents.Single((e) => e.Name == name);
                    }
                    catch
                    {
                        entry = new VmlEntry(name);
                        fileContents.Add(entry);
                    }

                    if (valueName.StartsWith("BIN:"))
                    {
                        if (value.GetType() == typeof(byte[]))
                        {
                            entry.Values.Add(new KeyValuePair<string, VmlSubEntry>(valueName, new VmlSubEntry(Bytes.BytesToHexString(value as byte[]), saveComments)));
                        }
                        else
                        {
                            entry.Values.Add(new KeyValuePair<string, VmlSubEntry>(valueName, new VmlSubEntry(Bytes.StringToHexStringUTF8(value.ToString()), saveComments)));
                        }
                    }
                    else
                    {
                        entry.Values.Add(new KeyValuePair<string, VmlSubEntry>(valueName, new VmlSubEntry(value, saveComments)));
                    }
                }
            }
        }

        private List<string> MakeComments(List<string> comments, bool sub)
        {
            List<string> commented = new List<string>();
            foreach (string comment in comments)
            {
                commented.Add((sub ? "\t;" : ";") + comment);
            }
            return commented;
        }

        /// <summary>
        /// Saves a vnml document.
        /// </summary>
        /// <param name="fileName">A file to which the vnml document should be saved to.</param>
        public void Save(string fileName)
        {
            List<string> lines = new List<string>();
            lines.Add(VersioStr[VersioStr.Length - 1]);
            foreach (VmlEntry entry in fileContents)
            {
                lines.AddRange(MakeComments(entry.Comments, false));
                lines.Add(entry.Name);
                foreach (KeyValuePair<string, VmlSubEntry> values in entry.Values)
                {
                    lines.AddRange(MakeComments(values.Value.Comments, true));
                    lines.Add("\t" + values.Key + "=[" + values.Value.Value.ToString() + "]");
                }
                lines.Add(string.Empty);
            }
            File.WriteAllLines(fileName, lines.ToArray(), Encoding.UTF8);
        }
    }
}
