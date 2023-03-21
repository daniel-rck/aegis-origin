﻿// <copyright file="GeoJsonObject.cs" company="Eötvös Loránd University (ELTE)">
//     Copyright (c) 2011-2023 Roberto Giachetta. Licensed under the
//     Educational Community License, Version 2.0 (the "License"); you may
//     not use this file except in compliance with the License. You may
//     obtain a copy of the License at
//     http://opensource.org/licenses/ECL-2.0
// 
//     Unless required by applicable law or agreed to in writing,
//     software distributed under the License is distributed on an "AS IS"
//     BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
//     or implied. See the License for the specific language governing
//     permissions and limitations under the License.
// </copyright>

namespace ELTE.AEGIS.IO.GeoJSON
{
    /// <summary>
    /// A base class of all readable GeoJSON object from the GeoJSON file.
    /// </summary>
    /// <author>Norbert Vass</author>
    public abstract class GeoJsonObject
    {
        /// <summary>
        /// The type of the GeoJSON object.
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// The object's Coordinate Reference System.
        /// </summary>
        public CrsObject Crs { get; set; }
        /// <summary>
        /// The object's Bounding Box array.
        /// </summary>
        public double[] BBox { get; set; }
    }
}
