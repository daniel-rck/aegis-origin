﻿/// <copyright file="GraphBasedSegmentation.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Algorithms;
using ELTE.AEGIS.Collections;
using ELTE.AEGIS.Collections.Segmentation;
using ELTE.AEGIS.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Operations.Spectral.Segmentation
{
    /// <summary>
    /// Represents an operation performing graph based segmentation on spectral geometries.
    /// </summary>
    public class GraphBasedSegmentation : SpectralSegmentation
    {
        #region Private types

        /// <summary>
        /// Simple graph edge representation. The source and destination vertexes are just indexes
        /// </summary>
        public class SimpleGraphEdge
        {
            /// <summary>
            /// The index of the source.
            /// </summary>
            public Int32 Source { get; private set; }

            /// <summary>
            /// The index of the destination.
            /// </summary>
            public Int32 Destination { get; private set; }

            /// <summary>
            /// The weight of the edge.
            /// </summary>
            public Double Weight { get; private set; }

            /// <summary>
            /// Initializes a new instance of the <see cref="SimpleGraphEdge"/> class.
            /// </summary>
            /// <param name="source">The source.</param>
            /// <param name="destination">The destination.</param>
            /// <param name="weight">The weight.</param>
            public SimpleGraphEdge(Int32 source, Int32 destination, Double weight)
            {
                Source = source;
                Destination = destination;
                Weight = weight;
            }
        }

        #endregion

        #region Private fields

        /// <summary>
        /// The segment merge threshold.
        /// </summary>
        private Double _mergeThreshold;

        /// <summary>
        /// The dictionary of innder difference values.
        /// </summary>
        private Dictionary<Segment, Double> _innerDiffences; 

        #endregion  

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphBasedSegmentation" /> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="method">The method.</param>
        /// <param name="parameters">The parameters.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The source is null.
        /// or
        /// The method is null.
        /// or
        /// The method requires parameters which are not specified.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The parameters do not contain a required parameter value.
        /// or
        /// The type of a parameter does not match the type specified by the method.
        /// or
        /// The parameter value does not satisfy the conditions of the parameter.
        /// </exception>
        public GraphBasedSegmentation(ISpectralGeometry source, IDictionary<OperationParameter, Object> parameters)
            : this(source, null, parameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphBasedSegmentation" /> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <param name="method">The method.</param>
        /// <param name="parameters">The parameters.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The source is null.
        /// or
        /// The method is null.
        /// or
        /// The method requires parameters which are not specified.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// The parameters do not contain a required parameter value.
        /// or
        /// The type of a parameter does not match the type specified by the method.
        /// or
        /// The parameter value does not satisfy the conditions of the parameter.
        /// or
        /// The specified source and result are the same objects, but the method does not support in-place operations.
        /// </exception>
        public GraphBasedSegmentation(ISpectralGeometry source, SegmentCollection target, IDictionary<OperationParameter, Object> parameters)
            : base(source, target, SpectralOperationMethods.GraphBasedSegmentation, parameters)
        {
            _mergeThreshold = Convert.ToDouble(ResolveParameter(SpectralOperationParameters.SegmentMergeThreshold));
        }

        #endregion

        #region Protected Operation methods

        /// <summary>
        /// Computes the result of the operation.
        /// </summary>
        protected override void ComputeResult()
        {
            // source: Felzenszwalb, Huttenlocher: Efficient Graph-Based Image Segmentation

            List<SimpleGraphEdge> edges = GenerateEdgeList();
            _innerDiffences = new Dictionary<Segment, Double>();
            
            // remove the edges and merge segments
            while (edges.Count > 0)
            {
                SimpleGraphEdge edge = edges[edges.Count - 1];
                edges.RemoveAt(edges.Count - 1);

                Segment firstSegment = _result[edge.Source / _source.Raster.NumberOfColumns, edge.Source % _source.Raster.NumberOfColumns];
                Segment secondSegment = _result[edge.Destination / _source.Raster.NumberOfColumns, edge.Destination % _source.Raster.NumberOfColumns];

                // if the two indices are already within the same segment
                if (firstSegment == secondSegment)
                    continue;

                Double internalDifference = ComputeMaximumInternalDifference(firstSegment, secondSegment);

                // if the weight of the edge does not influence the internal difference
                if (internalDifference > edge.Weight)
                {
                    // the segments should be merged
                    _result.MergeSegments(firstSegment, secondSegment);

                    // modify internal difference
                    Double weight = edge.Weight;
                    if (_innerDiffences.ContainsKey(secondSegment))
                    {
                        weight = Math.Max(_innerDiffences[secondSegment], edge.Weight);
                        _innerDiffences.Remove(secondSegment);
                    }

                    if (!_innerDiffences.ContainsKey(firstSegment))
                        _innerDiffences.Add(firstSegment, edge.Weight);
                    else
                        _innerDiffences[firstSegment] = Math.Max(_innerDiffences[firstSegment], weight);
                }
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Generates the initial edge list.
        /// </summary>
        /// <returns>The sorted edge list.</returns>
        private List<SimpleGraphEdge> GenerateEdgeList()
        {
            List<SimpleGraphEdge> edges = new List<SimpleGraphEdge>();

            // compute edge weight
            for (Int32 rowIndex = 0; rowIndex < _source.Raster.NumberOfRows; rowIndex++)
            {
                for (Int32 columnIndex = 0; columnIndex < _source.Raster.NumberOfColumns; columnIndex++)
                {
                    Int32 index = rowIndex * _source.Raster.NumberOfColumns + columnIndex;
                    Double weight;

                    switch (Source.Raster.Format)
                    { 
                        case RasterFormat.Integer:                            
                            if (columnIndex < _source.Raster.NumberOfColumns - 1)
                            {
                                weight = _distance.Distance(_source.Raster.GetValues(rowIndex, columnIndex), _source.Raster.GetValues(rowIndex, columnIndex + 1));
                                edges.Add(new SimpleGraphEdge(index, index + 1, weight));
                            }

                            if (columnIndex > 0)
                            {
                                weight = _distance.Distance(_source.Raster.GetValues(rowIndex, columnIndex), _source.Raster.GetValues(rowIndex, columnIndex - 1));
                                edges.Add(new SimpleGraphEdge(index, index - 1, weight));
                            }

                            if (rowIndex > 0)
                            {
                                weight = _distance.Distance(_source.Raster.GetValues(rowIndex, columnIndex), _source.Raster.GetValues(rowIndex - 1, columnIndex));
                                edges.Add(new SimpleGraphEdge(index, index - _source.Raster.NumberOfColumns, weight));
                            }

                            if (rowIndex < _source.Raster.NumberOfRows - 1)
                            {
                                weight = _distance.Distance(_source.Raster.GetValues(rowIndex, columnIndex), _source.Raster.GetValues(rowIndex + 1, columnIndex));
                                edges.Add(new SimpleGraphEdge(index, index + _source.Raster.NumberOfColumns, weight));
                            }
                            break;
                        case RasterFormat.Floating:
                            if (columnIndex < _source.Raster.NumberOfColumns - 1)
                            {
                                weight = _distance.Distance(_source.Raster.GetFloatValues(rowIndex, columnIndex), _source.Raster.GetFloatValues(rowIndex, columnIndex + 1));
                                edges.Add(new SimpleGraphEdge(index, index + 1, weight));
                            }

                            if (columnIndex > 0)
                            {
                                weight = _distance.Distance(_source.Raster.GetFloatValues(rowIndex, columnIndex), _source.Raster.GetFloatValues(rowIndex, columnIndex - 1));
                                edges.Add(new SimpleGraphEdge(index, index - 1, weight));
                            }

                            if (rowIndex > 0)
                            {
                                weight = _distance.Distance(_source.Raster.GetFloatValues(rowIndex, columnIndex), _source.Raster.GetFloatValues(rowIndex - 1, columnIndex));
                                edges.Add(new SimpleGraphEdge(index, index - _source.Raster.NumberOfColumns, weight));
                            }

                            if (rowIndex < _source.Raster.NumberOfRows - 1)
                            {
                                weight = _distance.Distance(_source.Raster.GetFloatValues(rowIndex, columnIndex), _source.Raster.GetFloatValues(rowIndex + 1, columnIndex));
                                edges.Add(new SimpleGraphEdge(index, index + _source.Raster.NumberOfColumns, weight));
                            }
                            break;
                    }

                }
            }

            // the edges are sorted by weight in descending order
            edges.Sort((x, y) => -x.Weight.CompareTo(y.Weight));

            return edges;
        }

        /// <summary>
        /// Computes the maximum internal difference of two segments.
        /// </summary>
        /// <param name="firstSegment">The first segment.</param>
        /// <param name="secondSegment">The second segment.</param>
        /// <returns>The maximum internal difference of the two segments.</returns>
        private Double ComputeMaximumInternalDifference(Segment firstSegment, Segment secondSegment)
        {
            return Math.Min(InternalDifference(firstSegment) + _mergeThreshold / firstSegment.Count, InternalDifference(secondSegment) + _mergeThreshold / secondSegment.Count);
        }
        
        /// <summary>
        /// Returns the internal difference of the specified segment.
        /// </summary>
        /// <param name="segment">The segment.</param>
        /// <returns>The internal difference of <paramref name="segment" />.</returns>
        private Double InternalDifference(Segment segment)
        {
            Double value;
            if (!_innerDiffences.TryGetValue(segment, out value))
                return 0;

            return value;
        }

        #endregion
    }
}