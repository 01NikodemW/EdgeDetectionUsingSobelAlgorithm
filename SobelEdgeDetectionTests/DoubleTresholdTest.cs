using System;
using SobelEdgeDetection;
using Xunit;

namespace SobelEdgeDetectionTests
{
    public class DoubleThresholdTest
    {
        [Fact]
        public void DoubleThresholdNullTest()
        {
            try
            {
                var doubleThreshold = new DoubleThreshold(null, 1, 2);
            }
            catch (NullReferenceException e)
            {
                Assert.True(true);
                return;
            }

            Assert.True(false);
        }

        [Fact]
        public void DoubleThresholdWrongBoundsTest()
        {
            var inputMatrix = new byte[,]
            {
                { 0, 0 },
                { 0, 0 }
            };
            try
            {
                var doubleThreshold = new DoubleThreshold(inputMatrix, 10, 5);
            }
            catch (ArgumentException e)
            {
                Assert.True(true);
                return;
            }

            Assert.True(false);
        }


        [Fact]
        public void ProperPixelTagginTest()
        {
            var inputMatrix = new byte[,]
            {
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 205, 149, 0, 0, 136, 0 },
                { 0, 0, 0, 0, 235, 0, 102, 122, 0, 0 },
                { 0, 0, 0, 0, 248, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 240, 0, 0, 0, 222, 0 },
                { 0, 0, 0, 0, 239, 221, 230, 220, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 73, 0, 0, 0, 0, 0, 245, 0 },
                { 0, 0, 110, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
            };

            byte lowerBound = 100;
            byte upperBound = 200;
            var doubleTreshold = new DoubleThreshold(inputMatrix, lowerBound, upperBound);
            var outputMatrix = doubleTreshold.CreateDoubleThresholdMatrix();

            for (var i = 0; i < inputMatrix.GetLength(0); i++)
            {
                for (var j = 0; j < inputMatrix.GetLength(1); j++)
                {
                    if (inputMatrix[i, j] < lowerBound)
                    {
                        if (outputMatrix[i, j].PixelType != DoubleThresholdPixelType.None)
                        {
                            Assert.True(false);
                            return;
                        }
                    }
                    else if (inputMatrix[i, j] < upperBound)
                    {
                        if (outputMatrix[i, j].PixelType != DoubleThresholdPixelType.Weak)
                        {
                            Assert.True(false);
                            return;
                        }
                    }
                    else
                    {
                        if (outputMatrix[i, j].PixelType != DoubleThresholdPixelType.Strong)
                        {
                            Assert.True(false);
                            return;
                        }
                    }
                }
            }

            Assert.True(true);
        }
    }
}