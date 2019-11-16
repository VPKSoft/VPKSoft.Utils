﻿#region License
/*
VPKSoft.Utils

Some utilities by VPKSoft.
Copyright (C) 2019 VPKSoft, Petteri Kautonen

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

using System.IO;

namespace VPKSoft.Utils
{
    /// <summary>
    /// A class to contain binary object serialization data.
    /// </summary>
    public class BinarySerializationContainer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BinarySerializationContainer"/> class.
        /// </summary>
        /// <param name="stream">The stream to get the binary serialization data.</param>
        public BinarySerializationContainer(Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
            var length = (int) stream.Length;
            BinaryData = new byte[length];
            stream.Read(BinaryData, 0, length);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BinarySerializationContainer"/> class.
        /// </summary>
        /// <param name="binaryData">The binary serialization data.</param>
        public BinarySerializationContainer(byte [] binaryData)
        {
            BinaryData = binaryData;
        }

        /// <summary>
        /// Gets or sets the binary serialization data.
        /// </summary>
        public byte [] BinaryData { get; set; }
    }
}