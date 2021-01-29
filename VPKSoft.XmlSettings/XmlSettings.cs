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
using System.Reflection;
using System.Xml.Linq;
using VPKSoft.Utils.XmlSettingsMisc;

namespace VPKSoft.Utils
{
    /// <summary>
    /// A base class to store settings to a user specified XML file.
    /// </summary>
    public abstract class XmlSettings
    {
        /// <summary>
        /// Loads the settings from the given <see cref="XDocument"/> document.
        /// </summary>
        /// <returns>XDocument containing the loaded data.</returns>
        public XDocument Load(XDocument document)
        {
            // get all public instance properties of this class..
            PropertyInfo[] propertyInfos = GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);

            // loop through the properties..
            foreach (var propertyInfo in propertyInfos)
            {
                try // avoid crashes..
                {
                    // get the IsSetting class instance of the property..
                    var isSettingAttribute = (IsSetting) propertyInfo.GetCustomAttribute(typeof(IsSetting));

                    // only properties marked with the IsSetting will be handled..
                    if (isSettingAttribute == null)
                    {
                        continue;
                    }

                    // get the default value for the property..
                    object currentValue = propertyInfo.GetValue(this);

                    var type = propertyInfo.PropertyType;

                    var element = document.Root?.Elements()
                        .FirstOrDefault(f =>
                            f.Attribute(propertyInfo.Name) != null && 
                            f.Attribute(propertyInfo.Name)?.Value != null &&
                            f.Name == "setting");

                    if (element != null)
                    {
                        var value = element.Attribute(propertyInfo.Name)?.Value;
                        var secure = element.Attribute("secure")?.Value != null &&
                                     element.Attribute("secure")?.Value == "1";

                        if (secure)
                        {
                            var args = new RequestDecryptionEventArgs {SettingName = propertyInfo.Name, Value = value};
                            RequestDecryption?.Invoke(this, args);
                            value = args.Value;
                        }

                        if (type.IsPrimitive || type == typeof(string))
                        {
                            currentValue = Convert.ChangeType(value, type);
                        }
                        else
                        {
                            var args = new RequestTypeConverterEventArgs
                                {SettingName = propertyInfo.Name, TypeToConvert = type};

                            RequestTypeConverter?.Invoke(this, args);

                            if (args.TypeConverter != null)
                            {
                                currentValue = args.TypeConverter.ConvertFromString(value);
                            }
                        }
                    }
                    else if (isSettingAttribute.HasDefaultValue && isSettingAttribute.DefaultValue != GetDefaultValue(type))
                    {
                        currentValue = isSettingAttribute.DefaultValue;
                    }

                    if (!DefaultValuesSet.Contains(propertyInfo.Name))
                    {
                        DefaultValuesSet.Add(propertyInfo.Name);
                    }

                    // set the value for the property..
                    propertyInfo.SetValue(this, currentValue);
                }
                catch (Exception ex)
                {
                    // inform of the exception..
                    ReportExceptionAction?.Invoke(ex);
                }
            }

            return document;
        }

        /// <summary>
        /// Get a default value of a given <see cref="Type"/> value.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> of which default value to get.</param>
        /// <returns>The default value of a given <see cref="Type"/>.</returns>
        private static object GetDefaultValue(Type type)
        {
            try
            {
                if (Nullable.GetUnderlyingType(type) != null)
                {
                    return null;
                }
                
                return type.IsValueType ? Activator.CreateInstance(type) : null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Loads the settings from the given XML file.
        /// </summary>
        /// <param name="fileName">Name of the file to load the settings from.</param>
        /// <returns>XDocument containing the loaded data.</returns>
        public XDocument Load(string fileName)
        {
            try
            {
                XDocument document = XDocument.Load(fileName);
                Load(document);
                return document;
            }
            catch (Exception ex)
            {
                // inform of the exception..
                ReportExceptionAction?.Invoke(ex);
            }

            return null;
        }

        /// <summary>
        /// Saves the settings to a specified file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>The XDocument containing the saved settings.</returns>
        public XDocument Save(string fileName)
        {
            var document = Save();
            try
            {
                document.Save(fileName);
            }
            catch (Exception ex)
            {
                // inform of the exception..
                ReportExceptionAction?.Invoke(ex);
            }
            return document;
        }

        // a list containing the settings the default value has already been set..
        private List<string> DefaultValuesSet { get; set; } = new List<string>();

        /// <summary>
        /// Saves the settings to a <see cref="XDocument"/> instance.
        /// </summary>
        /// <returns>The XDocument containing the saved settings.</returns>
        public XDocument Save()
        {
            // get all public instance properties of this class..
            PropertyInfo[] propertyInfos = GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);

            // create an element for the setting value entries..
            XElement settingsElement = new XElement("settings");

            // loop through the properties..
            foreach (var propertyInfo in propertyInfos)
            {
                try // avoid crashes..
                {
                    // get the IsSetting class instance of the property..
                    var isSettingAttribute = (IsSetting) propertyInfo.GetCustomAttribute(typeof(IsSetting));

                    // only properties marked with the IsSetting will be handled..
                    if (isSettingAttribute == null)
                    {
                        continue;
                    }

                    // get the default value for the property..
                    object currentValue = propertyInfo.GetValue(this);

                    if (currentValue == null && isSettingAttribute.DefaultValue == null) // the null case..
                    {
                        continue;
                    }

                    var type = propertyInfo.PropertyType;
                    
                    if (!DefaultValuesSet.Contains(propertyInfo.Name) && 
                        isSettingAttribute.DefaultValue != null && 
                        isSettingAttribute.DefaultValue != GetDefaultValue(type) && 
                        GetDefaultValue(type) != currentValue)
                    {
                        currentValue = isSettingAttribute.DefaultValue;
                    }

                    if (!DefaultValuesSet.Contains(propertyInfo.Name))
                    {
                        DefaultValuesSet.Add(propertyInfo.Name);
                    }

                    string value = null;

                    if (type.IsPrimitive || type == typeof(string))
                    {
                        value = currentValue?.ToString();
                    }
                    else
                    {
                        var args = new RequestTypeConverterEventArgs
                            {SettingName = propertyInfo.Name, TypeToConvert = type};

                        RequestTypeConverter?.Invoke(this, args);

                        if (args.TypeConverter != null)
                        {
                            value = args.TypeConverter.ConvertToString(currentValue);
                        }
                    }

                    if (isSettingAttribute.Secure)
                    {
                        var args = new RequestEncryptionEventArgs {SettingName = propertyInfo.Name, Value = value};
                        RequestEncryption?.Invoke(this, args);
                        value = args.Value;
                    }

                    var settingElement = new XElement("setting", 
                        new XAttribute(propertyInfo.Name, value),
                        new XAttribute("secure", isSettingAttribute.Secure ? "1" : "0"));

                    settingsElement.Add(settingElement);
                }
                catch (Exception ex)
                {
                    // inform of the exception..
                    ReportExceptionAction?.Invoke(ex);
                }
            }

            return new XDocument(new XDeclaration("1.0", "utf-8", ""), settingsElement);
        }

        /// <summary>
        /// An action which is called when an exception occurs within the class.
        /// </summary>
        public Action<Exception> ReportExceptionAction { get; set; }

        /// <summary>
        /// A delegate for the <see cref="RequestEncryption"/> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RequestEncryptionEventArgs"/> instance containing the event data.</param>
        public delegate void OnRequestEncryption(object sender, RequestEncryptionEventArgs e);

        /// <summary>
        /// Occurs when a setting value is required to be encrypted.
        /// </summary>
        public event OnRequestEncryption RequestEncryption;

        /// <summary>
        /// A delegate for the <see cref="RequestDecryption"/> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RequestDecryptionEventArgs"/> instance containing the event data.</param>
        public delegate void OnRequestDecryption(object sender, RequestDecryptionEventArgs e);

        /// <summary>
        /// Occurs when a setting value is required to be decrypted.
        /// </summary>
        public event OnRequestDecryption RequestDecryption;

        /// <summary>
        /// A delegate for the <see cref="RequestTypeConverter"/> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RequestTypeConverterEventArgs"/> instance containing the event data.</param>
        public delegate void OnRequestTypeConverter(object sender, RequestTypeConverterEventArgs e);

        /// <summary>
        /// Occurs when the setting value is complex type and a type converter is required for the string value conversion.
        /// </summary>
        public event OnRequestTypeConverter RequestTypeConverter;
    }
}
