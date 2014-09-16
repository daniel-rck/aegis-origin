﻿/// <copyright file="NearestNeighbourResamplingStrategyTest.cs" company="Eötvös Loránd University (ELTE)">
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

using ELTE.AEGIS.Operations.Spectral.Resampling.Strategy;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;

namespace ELTE.AEGIS.Tests.Operations.Spectral.Resampling.Strategy
{
    /// <summary>
    /// Test fixture for the <see cref="NearestNeighbourResamplingStrategy" /> class.
    /// </summary>
    [TestFixture]
    public class NearestNeighbourResamplingStrategyTest
    {
        #region Private fields

        /// <summary>
        /// The mock of the raster.
        /// </summary>
        private Mock<IRaster> _rasterMock;

        #endregion

        #region Test setup

        /// <summary>
        /// Test setup.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            _rasterMock = new Mock<IRaster>(MockBehavior.Strict);
            _rasterMock.Setup(raster => raster.NumberOfRows).Returns(20);
            _rasterMock.Setup(raster => raster.NumberOfColumns).Returns(15);
            _rasterMock.Setup(raster => raster.NumberOfBands).Returns(3);
            _rasterMock.Setup(raster => raster.Format).Returns(RasterFormat.Integer);
            _rasterMock.Setup(raster => raster.GetValue(It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<Int32>()))
                                              .Returns(new Func<Int32, Int32, Int32, UInt32>((rowIndex, columnIndex, bandIndex) => (UInt32)((rowIndex * columnIndex * bandIndex + 256) % 256)));
            _rasterMock.Setup(raster => raster.GetValues(It.IsAny<Int32>(), It.IsAny<Int32>()))
                                              .Returns(new Func<Int32, Int32, UInt32[]>((rowIndex, columnIndex) => Enumerable.Range(0, 3).Select(bandIndex => (UInt32)(rowIndex * columnIndex * bandIndex + 256) % 256).ToArray()));
            _rasterMock.Setup(raster => raster.GetNearestValue(It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<Int32>()))
                                              .Returns(new Func<Int32, Int32, Int32, UInt32>((rowIndex, columnIndex, bandIndex) => (UInt32)((rowIndex * columnIndex * bandIndex + 256) % 256)));
            _rasterMock.Setup(raster => raster.GetNearestValues(It.IsAny<Int32>(), It.IsAny<Int32>()))
                                              .Returns(new Func<Int32, Int32, UInt32[]>((rowIndex, columnIndex) => Enumerable.Range(0, 3).Select(bandIndex => (UInt32)(rowIndex * columnIndex * bandIndex + 256) % 256).ToArray()));
            _rasterMock.Setup(raster => raster.GetFloatValue(It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<Int32>()))
                                              .Returns(new Func<Int32, Int32, Int32, Double>((rowIndex, columnIndex, bandIndex) => (rowIndex * columnIndex * bandIndex + 256) % 256));
            _rasterMock.Setup(raster => raster.GetFloatValues(It.IsAny<Int32>(), It.IsAny<Int32>()))
                                              .Returns(new Func<Int32, Int32, Double[]>((rowIndex, columnIndex) => Enumerable.Range(0, 3).Select(bandIndex => (Double)(rowIndex * columnIndex * bandIndex + 256) % 256).ToArray()));
            _rasterMock.Setup(raster => raster.GetNearestFloatValue(It.IsAny<Int32>(), It.IsAny<Int32>(), It.IsAny<Int32>()))
                                              .Returns(new Func<Int32, Int32, Int32, Double>((rowIndex, columnIndex, bandIndex) => (rowIndex * columnIndex * bandIndex + 256) % 256));
            _rasterMock.Setup(raster => raster.GetNearestFloatValues(It.IsAny<Int32>(), It.IsAny<Int32>()))
                                              .Returns(new Func<Int32, Int32, Double[]>((rowIndex, columnIndex) => Enumerable.Range(0, 3).Select(bandIndex => (Double)(rowIndex * columnIndex * bandIndex + 256) % 256).ToArray()));
        }

        #endregion

        #region Test methods

        /// <summary>
        /// Test case for integer value computation.
        /// </summary>
        [Test]
        public void NearestNeighbourResamplingStrategyComputeTest()
        {
            NearestNeighbourResamplingStrategy strategy = new NearestNeighbourResamplingStrategy(_rasterMock.Object);

                for (Double rowIndex = 0; rowIndex < _rasterMock.Object.NumberOfRows; rowIndex += 0.1)
                    for (Double columnIndex = 0; columnIndex < _rasterMock.Object.NumberOfColumns; columnIndex += 0.1)
                    {
                        // individual bands
                        for (Int32 bandIndex = 0; bandIndex < _rasterMock.Object.NumberOfBands; bandIndex++)
                        {
                            Assert.AreEqual(_rasterMock.Object.GetValue((Int32)Math.Round(rowIndex), (Int32)Math.Round(columnIndex), bandIndex), strategy.Compute(rowIndex, columnIndex, bandIndex));
                        }

                        // all bands
                        Assert.IsTrue(_rasterMock.Object.GetValues((Int32)Math.Round(rowIndex), (Int32)Math.Round(columnIndex)).SequenceEqual(strategy.Compute(rowIndex, columnIndex)));
                    }
        }

        /// <summary>
        /// Test case for floating value computation.
        /// </summary>
        [Test]
        public void NearestNeighbourResamplingStrategyComputeFloatTest()
        {
            NearestNeighbourResamplingStrategy strategy = new NearestNeighbourResamplingStrategy(_rasterMock.Object);

            for (Double rowIndex = 0; rowIndex < _rasterMock.Object.NumberOfRows; rowIndex += 0.1)
                for (Double columnIndex = 0; columnIndex < _rasterMock.Object.NumberOfColumns; columnIndex += 0.1)
                {
                    // individual bands
                    for (Int32 bandIndex = 0; bandIndex < _rasterMock.Object.NumberOfBands; bandIndex++)
                    {
                        Assert.AreEqual(_rasterMock.Object.GetFloatValue((Int32)Math.Round(rowIndex), (Int32)Math.Round(columnIndex), bandIndex), strategy.ComputeFloat(rowIndex, columnIndex, bandIndex));
                    }

                    // all bands
                    Assert.IsTrue(_rasterMock.Object.GetFloatValues((Int32)Math.Round(rowIndex), (Int32)Math.Round(columnIndex)).SequenceEqual(strategy.ComputeFloat(rowIndex, columnIndex)));
                }
        }

        #endregion
    }
}
