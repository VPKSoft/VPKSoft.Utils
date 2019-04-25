#region License
/*
VPKSoft.Utils

Some utilities by VPKSoft.
Copyright (C) 2018 VPKSoft, Petteri Kautonen

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
using System.Xml.Serialization;
using System.IO;
using System.Xml;

namespace VPKSoft.Utils
{
    /// <summary>
    /// A class to serialize and to deserialize objects in a form of a XML data strings or XmlDocument class instances. More will be added.
    /// </summary>
    public static class ObjectSerialization
    {
        // THIS: (C): https://stackoverflow.com/questions/2434534/serialize-an-object-to-string
        #region MethodLicense
        /*
        <A library to serialize and deserialize class instances using XmlSerializer class. (c) by Petteri Kautonen

        A library to serialize and deserialize class instances using XmlSerializer class is licensed under a
        Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License.

        You should have received a copy of the license along with this
        work.  If not, see <http://creativecommons.org/licenses/by-nc-sa/3.0/>.
        */


        // https://creativecommons.org/licenses/by-sa/3.0/
        #endregion
        /// <summary>
        /// A helper to serialize an object to a string containing XML data of the object.
        /// </summary>
        /// <typeparam name="T">An object to serialize to a XML data string.</typeparam>
        /// <param name="toSerialize">A helper method for any type of object to be serialized to a XML data string.</param>
        /// <returns>A string containing XML data of the object.</returns>
        public static string SerializeObject<T>(this T toSerialize)
        {
            // create an instance of a XmlSerializer class with the typeof(T)..
            XmlSerializer xmlSerializer = new XmlSerializer(toSerialize.GetType());

            // using is necessary with classes which implement the IDisposable interface..
            using (StringWriter stringWriter = new StringWriter())
            {
                // serialize a class to a StringWriter class instance..
                xmlSerializer.Serialize(stringWriter, toSerialize); // a base class of the StringWriter instance is TextWriter..
                return stringWriter.ToString(); // return the value..
            }
        }

        // THIS: (C): VPKSoft, 2018, https://www.vpksoft.net
        /// <summary>
        /// A helper to serialize an object to a XmlDocument class instance.
        /// </summary>
        /// <typeparam name="T">>An object of type of T to serialize from a XmlDocument class instance.</typeparam>
        /// <param name="toXml">An object to serialize to a XmlDocument class instance.</param>
        /// <returns>A XmlDocument class instance containing the deserialized object data.</returns>
        public static XmlDocument ToXmlDocument<T>(this T toXml)
        {
            // create an instance of a XmlSerializer class with the typeof(T)..
            XmlSerializer xmlSerializer = new XmlSerializer(toXml.GetType());

            // create a memory stream to save the XML data..
            using (MemoryStream ms = new MemoryStream())
            {
                xmlSerializer.Serialize(ms, toXml); // serialize the object to the memory stream..
                XmlDocument xmlDocument = new XmlDocument(); // create an XML document instance..
                ms.Position = 0; // just in case set the stream's position to 0..
                xmlDocument.Load(ms); // load the XML document..
                return xmlDocument; // return the XmlDocument class instance..
            }
        }

        // THIS: (C): VPKSoft, 2018, https://www.vpksoft.net        
        /// <summary>
        /// Deserializes an object which is saved to a XmlDocument class instance. If the object has no instance a new object will be constructed if possible.
        /// <note type="note">An exception will occur if a null reference is called an no valid constructor of the class is available.</note>
        /// </summary>
        /// <typeparam name="T">An object of type of T to deserialize from a XmlDocument class instance.</typeparam>
        /// <param name="toDeserialize">An object to deserialize from a from a XmlDocument class instance.</param>
        /// <param name="xmlDocument">A XmlDocument class instance to deserialize the object from.</param>
        /// <returns>An object which is deserialized from the XmlDocument class instance.</returns>
        public static T DeserializeObject<T>(this T toDeserialize, XmlDocument xmlDocument)
        {
            // if a null instance of an object called this try to create a "default" instance for it with typeof(T),
            // this will throw an exception no useful constructor is found..
            object voidInstance = toDeserialize == null ? Activator.CreateInstance(typeof(T)) : toDeserialize;

            // create an instance of a XmlSerializer class with the typeof(T)..
            XmlSerializer xmlSerializer = new XmlSerializer(voidInstance.GetType());

            // create a memory stream to save the XML data..
            using (MemoryStream ms = new MemoryStream())
            {
                xmlDocument.Save(ms);
                ms.Position = 0; // just in case set the stream's position to 0..

                // deserialize the XML data to an object of type T and return the object..
                return (T)xmlSerializer.Deserialize(ms);
            }
        }


        // THIS: (C): VPKSoft, 2018, https://www.vpksoft.net
        /// <summary>
        /// Deserializes an object which is saved to an XML data string. If the object has no instance a new object will be constructed if possible.
        /// <note type="note">An exception will occur if a null reference is called an no valid constructor of the class is available.</note>
        /// </summary>
        /// <typeparam name="T">An object to deserialize from a XML data string.</typeparam>
        /// <param name="toDeserialize">An object of which XML data to deserialize. If the object is null a a default constructor is called.</param>
        /// <param name="xmlData">A string containing a serialized XML data do deserialize.</param>
        /// <returns>An object which is deserialized from the XML data string.</returns>
        public static T DeserializeObject<T>(this T toDeserialize, string xmlData)
        {
            // if a null instance of an object called this try to create a "default" instance for it with typeof(T),
            // this will throw an exception no useful constructor is found..
            object voidInstance = toDeserialize == null ? Activator.CreateInstance(typeof(T)) : toDeserialize;

            // create an instance of a XmlSerializer class with the typeof(T)..
            XmlSerializer xmlSerializer = new XmlSerializer(voidInstance.GetType());

            // construct a StringReader class instance of the given xmlData parameter to be deserialized by the XmlSerializer class instance..
            using (StringReader stringReader = new StringReader(xmlData))
            {
                // return the "new" object deserialized via the XmlSerializer class instance..
                return (T)xmlSerializer.Deserialize(stringReader);
            }
        }


        // THIS: (C): VPKSoft, 2018, https://www.vpksoft.net        
        /// <summary>
        /// Deserializes an object which is saved to a XmlDocument class instance. If the object has no instance a new object will be constructed if possible.
        /// <note type="note">An exception will occur if a null reference is called an no valid constructor of the class is available.</note>
        /// </summary>
        /// <param name="toDeserialize">A type of an object of which data to deserialize.</param>
        /// <param name="xmlDocument">A XmlDocument class instance to deserialize the object from.</param>
        /// <returns>An object which is deserialized from the XmlDocument class instance.</returns>
        public static object DeserializeObject(Type toDeserialize, XmlDocument xmlDocument)
        {
            // create an instance of a XmlSerializer class with the given type toDeserialize..
            XmlSerializer xmlSerializer = new XmlSerializer(toDeserialize);

            // create a memory stream to save the XML data..
            using (MemoryStream ms = new MemoryStream())
            {
                xmlDocument.Save(ms);
                ms.Position = 0; // just in case set the stream's position to 0..

                // deserialize the XML data to an object of type T and return the object..
                return xmlSerializer.Deserialize(ms);
            }
        }

        // THIS: (C): VPKSoft, 2018, https://www.vpksoft.net
        /// <summary>
        /// Deserializes an object which is saved to an XML data string.
        /// </summary>
        /// <param name="toDeserialize">A type of an object of which XML data to deserialize.</param>
        /// <param name="xmlData">A string containing a serialized XML data do deserialize.</param>
        /// <returns>An object which is deserialized from the XML data string.</returns>
        public static object DeserializeObject(Type toDeserialize, string xmlData)
        {
            // create an instance of a XmlSerializer class with the given type toDeserialize..
            XmlSerializer xmlSerializer = new XmlSerializer(toDeserialize);

            // construct a StringReader class instance of the given xmlData parameter to be deserialized by the XmlSerializer class instance..
            using (StringReader stringReader = new StringReader(xmlData))
            {
                // return the "new" object deserialized via the XmlSerializer class instance..
                return xmlSerializer.Deserialize(stringReader);
            }
        }
    }
}
