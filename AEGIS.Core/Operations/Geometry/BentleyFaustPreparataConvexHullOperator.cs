﻿///<copyright file="BentleyFaustPreparataConvexHullOperator.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2015 Roberto Giachetta. Licensed under the
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

using ELTE.AEGIS.Algorithms;
using System;

namespace ELTE.AEGIS.Operations.Geometry
{
    /// <summary>
    /// Represents an operator computing the convex hull using the Bentley-Faust-Preparata algorithm.
    /// </summary>
    public class BentleyFaustPreparataConvexHullOperator : IGeometryConvexHullOperator
    {
        #region IGeometryConvexHullOperator methods

        /// <summary>
        /// Computes the convex hull of the specified <see cref="IGeometry" /> instance.
        /// </summary>
        /// <param name="geometry">The geometry.</param>
        /// <returns>The convex hull of the <see cref="IGeometry" /> instance.</returns>
        /// <exception cref="System.ArgumentNullException">The geometry is null.</exception>
        /// <exception cref="System.ArgumentException">The operation is not supported with the specified geometry type.</exception>
        public IGeometry ConvexHull(IGeometry geometry)
        {
            if (geometry == null)
                throw new ArgumentNullException("geometry", "The geometry is null.");

            if (geometry is IPoint)
                return geometry.Clone() as IGeometry;
            if (geometry is ICurve)
                return geometry.Factory.CreatePolygon(BentleyFaustPreparataAlgorithm.ComputeApproximateConvexHull((geometry as ILineString).Coordinates));
            if (geometry is IPolygon)
                return geometry.Factory.CreatePolygon(BentleyFaustPreparataAlgorithm.ComputeApproximateConvexHull((geometry as IPolygon).Shell.Coordinates));

            throw new ArgumentException("The operation is not supported with the specified geometry type.");
        }

        #endregion

        #region IDisposable methods

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose() { }

        #endregion
    }
}