﻿/// <copyright file="BentleyOttmannAlgorithmTest.cs" company="Eötvös Loránd University (ELTE)">
///     Copyright (c) 2011-2014 Robeto Giachetta. Licensed under the
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
/// <author>Máté Cserép</author>

using ELTE.AEGIS.Algorithms;
using ELTE.AEGIS.Numerics;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ELTE.AEGIS.Tests.Core.Algorithms
{
    /// <summary>
    /// Test fixture for the <see cref="BentleyOttmannAlgorithm" /> class.
    /// </summary>
    [TestFixture]
    public class BentleyOttmannAlgorithmTest
    {
        #region Private fields

        /// <summary>
        /// The geometry factory.
        /// </summary>
        private IGeometryFactory _factory;

        #endregion

        #region Test setup.

        /// <summary>
        /// Test fixture setup.
        /// </summary>
        [TestFixtureSetUp]
        public void FixtureInitialize()
        {
            _factory = Factory.DefaultInstance<IGeometryFactory>();
        }

        #endregion

        #region Test cases

        /// <summary>
        /// Test case for the <see cref="Intersection" /> method.
        /// </summary>
        [Test]
        public void BentleyOttmannAlgorithmIntersectionTest()
        {
            // single line, no intersection

            IList<Coordinate> intersections = BentleyOttmannAlgorithm.Intersection(new List<Coordinate>
                {
                    new Coordinate(10, 10),
                    new Coordinate(20, 20)
                });
            Assert.AreEqual(intersections.Count, 0);


            // single linestring, one intersection

            intersections = BentleyOttmannAlgorithm.Intersection(new List<Coordinate>
                {
                    new Coordinate(10, 10),
                    new Coordinate(20, 20),
                    new Coordinate(15, 20),
                    new Coordinate(15, 10)
                });
            Assert.AreEqual(intersections.Count, 1);
            Assert.AreEqual(intersections[0], new Coordinate(15, 15));


            // multiple lines, no intersection

            intersections = BentleyOttmannAlgorithm.Intersection(new[]
                {
                    new List<Coordinate>
                        {
                            new Coordinate(10, 10),
                            new Coordinate(20, 20)
                        },
                    new List<Coordinate>
                        {
                            new Coordinate(0, 0),
                            new Coordinate(10, 0)
                        }
                });
            Assert.AreEqual(intersections.Count, 0);


            // multiple lines, one intersection
            
            intersections = BentleyOttmannAlgorithm.Intersection(new[]
                {
                    new List<Coordinate>
                        {
                            new Coordinate(10, 10),
                            new Coordinate(20, 20)
                        },
                    new List<Coordinate>
                        {
                            new Coordinate(0, 0),
                            new Coordinate(10, 10)
                        }
                });
            Assert.AreEqual(intersections.Count, 1);
            Assert.AreEqual(intersections[0], new Coordinate(10, 10));


            // multiple lines, one intersection

            intersections = BentleyOttmannAlgorithm.Intersection(new[]
                {
                    new List<Coordinate>
                        {
                            new Coordinate(10, 10),
                            new Coordinate(20, 20)
                        },
                    new List<Coordinate>
                        {
                            new Coordinate(15, 20),
                            new Coordinate(15, 10)
                        }
                });
            Assert.AreEqual(intersections.Count, 1);
            Assert.AreEqual(intersections[0], new Coordinate(15, 15));
        

            // multiple lines, multiple intersections
            
            intersections = BentleyOttmannAlgorithm.Intersection(new[]
                {
                    new List<Coordinate>
                        {
                            new Coordinate(-10, 0),
                            new Coordinate(10, 0)
                        },
                    new List<Coordinate>
                        {
                            new Coordinate(-10, -10),
                            new Coordinate(10, 10)
                        },
                    new List<Coordinate>
                        {
                            new Coordinate(3, 5),
                            new Coordinate(10, 5)
                        },
                    new List<Coordinate>
                        {
                            new Coordinate(4, 8),
                            new Coordinate(10, 8)
                        }
                });
            Assert.AreEqual(intersections.Count, 3);
            Assert.AreEqual(intersections[0], new Coordinate(0, 0));
            Assert.AreEqual(intersections[1], new Coordinate(5, 5));
            Assert.AreEqual(intersections[2], new Coordinate(8, 8));


            // multiple lines, multiple intersections
            
            intersections = BentleyOttmannAlgorithm.Intersection(new[]
                {
                    new List<Coordinate>
                        {
                            new Coordinate(-5, 0),
                            new Coordinate(5, 0)
                        },
                    new List<Coordinate>
                        {
                            new Coordinate(0, -2),
                            new Coordinate(8, 2)
                        },
                    new List<Coordinate>
                        {
                            new Coordinate(1, -3),
                            new Coordinate(3, 3)
                        }
                });
            Assert.AreEqual(intersections.Count, 3);
            var result = intersections[0] - new Coordinate(1.6, -1.2);
            Assert.LessOrEqual(Math.Abs(result.X), Calculator.Tolerance);
            Assert.LessOrEqual(Math.Abs(result.Y), Calculator.Tolerance);
            Assert.AreEqual(intersections[1], new Coordinate(2, 0));
            Assert.AreEqual(intersections[2], new Coordinate(4, 0));


            // multiple lines, multiple intersections
            
            intersections = BentleyOttmannAlgorithm.Intersection(new[]
                {
                    new List<Coordinate>
                        {
                            new Coordinate(-5, 0),
                            new Coordinate(5, 0)
                        },
                    new List<Coordinate>
                        {
                            new Coordinate(0, 5),
                            new Coordinate(5, 0)
                        },
                    new List<Coordinate>
                        {
                            new Coordinate(4, -1),
                            new Coordinate(5, 0)
                        }
                });
            Assert.AreEqual(intersections.Count, 3);
            Assert.AreEqual(intersections[0], new Coordinate(5, 0));
            Assert.AreEqual(intersections[1], new Coordinate(5, 0));
            Assert.AreEqual(intersections[2], new Coordinate(5, 0));


            // multiple lines, multiple intersections
            
            intersections = BentleyOttmannAlgorithm.Intersection(new[]
                {
                    new List<Coordinate>
                        {
                            new Coordinate(10, 0),
                            new Coordinate(10, 10),
                            new Coordinate(0, 10)
                        },
                    new List<Coordinate>
                        {
                            new Coordinate(10, 20),
                            new Coordinate(10, 10),
                            new Coordinate(20, 10)
                        }
                });
            Assert.AreEqual(intersections.Count, 4);
            Assert.AreEqual(intersections.Count(i => i.Equals(new Coordinate(10, 10))), 4);


            // single polygon, no intersection
            
            intersections = BentleyOttmannAlgorithm.Intersection(
                _factory.CreatePolygon(
                    _factory.CreatePoint(0, 0),
                    _factory.CreatePoint(10, 0),
                    _factory.CreatePoint(10, 10),
                    _factory.CreatePoint(0, 10))
                        .Shell.Coordinates);
            Assert.AreEqual(intersections.Count, 0);


            // multiple polygons, no intersection
            
            intersections = BentleyOttmannAlgorithm.Intersection(new[]
                {
                    _factory.CreatePolygon(
                        _factory.CreatePoint(0, 0),
                        _factory.CreatePoint(10, 0),
                        _factory.CreatePoint(10, 10),
                        _factory.CreatePoint(0, 10))
                            .Shell.Coordinates,
                    _factory.CreatePolygon(
                        _factory.CreatePoint(15, 0),
                        _factory.CreatePoint(20, 0),
                        _factory.CreatePoint(20, 5),
                        _factory.CreatePoint(15, 5))
                            .Shell.Coordinates
                });
            Assert.AreEqual(intersections.Count, 0);


            // multiple polygons, multiple intersections
            
            intersections = BentleyOttmannAlgorithm.Intersection(new[]
                {
                    _factory.CreatePolygon(
                        _factory.CreatePoint(0, 0),
                        _factory.CreatePoint(10, 0),
                        _factory.CreatePoint(10, 10),
                        _factory.CreatePoint(0, 10))
                            .Shell.Coordinates,
                    _factory.CreatePolygon(
                        _factory.CreatePoint(10, 10),
                        _factory.CreatePoint(20, 10),
                        _factory.CreatePoint(20, 20),
                        _factory.CreatePoint(10, 20))
                            .Shell.Coordinates
                });
            Assert.AreEqual(intersections.Count, 4);
            Assert.AreEqual(intersections[0], new Coordinate(10, 10));
            Assert.AreEqual(intersections[1], new Coordinate(10, 10));
            Assert.AreEqual(intersections[2], new Coordinate(10, 10));
            Assert.AreEqual(intersections[3], new Coordinate(10, 10));


            // multiple polygons, multiple intersections

            intersections = BentleyOttmannAlgorithm.Intersection(new[]
                {
                    _factory.CreatePolygon(
                        _factory.CreatePoint(0, 0),
                        _factory.CreatePoint(10, 0),
                        _factory.CreatePoint(10, 10),
                        _factory.CreatePoint(0, 10))
                            .Shell.Coordinates,
                    _factory.CreatePolygon(
                        _factory.CreatePoint(5, 5),
                        _factory.CreatePoint(15, 5),
                        _factory.CreatePoint(15, 15),
                        _factory.CreatePoint(5, 15))
                            .Shell.Coordinates
                });
            Assert.AreEqual(intersections.Count, 2);
            Assert.AreEqual(intersections[0], new Coordinate(5, 10));
            Assert.AreEqual(intersections[1], new Coordinate(10, 5));


            // multiple polygons, multiple intersections
            
            intersections = BentleyOttmannAlgorithm.Intersection(new[]
                {
                    _factory.CreatePolygon(
                        _factory.CreatePoint(0, 0),
                        _factory.CreatePoint(10, 0),
                        _factory.CreatePoint(10, 10),
                        _factory.CreatePoint(0, 10))
                            .Shell.Coordinates,
                    _factory.CreatePolygon(
                        _factory.CreatePoint(-10, -5),
                        _factory.CreatePoint(10, -5),
                        _factory.CreatePoint(0, 5))
                            .Shell.Coordinates
                });
            Assert.AreEqual(intersections.Count, 3);
            Assert.AreEqual(intersections[0], new Coordinate(0, 5));
            Assert.AreEqual(intersections[1], new Coordinate(0, 5));
            Assert.AreEqual(intersections[2], new Coordinate(5, 0));
        }

        #endregion
    }
}
