﻿/// <copyright file="RasterInterpretationData.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Roberto Giachetta. Licensed under the
///     Educational Community License, Version 2.0 (the "License"); you may
///     not use this file except in compliance with the License. You may
///     obtain a copy of the License at
///     http://opensource.org/licenses/ECL-2.0
///
///     Unless required by applicable law or agreed to in writing,
///     software distributed under the License is distributed on an "AS IS"
///     BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
///     or implied. See the License for the specific language governing
///     permissions and limitations under the License.
/// </copyright>
/// <author>Roberto Giachetta</author>

using System;
using System.Collections.Generic;

namespace ELTE.AEGIS
{
    
    /// <summary>
    /// Represents a type containing interpretation data of raster images.
    /// </summary>
    public class RasterInterpretation
    {
        #region Private fields

        private RasterColorSpaceChannel[] _channels;

        #endregion

        #region Public properties

        public RasterColorSpace ColorSpace { get; private set; }

        public IList<RasterColorSpaceChannel> Channels { get { return Array.AsReadOnly(_channels); } }

        #endregion

        #region Constructors

        public RasterInterpretation(RasterColorSpace colorSpace, params RasterColorSpaceChannel[] channels)
        {
            ColorSpace = colorSpace;
            _channels = channels;
        }

        #endregion
    }
}