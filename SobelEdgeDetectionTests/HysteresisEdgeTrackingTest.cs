using System;
using SobelEdgeDetection;
using Xunit;

namespace SobelEdgeDetectionTests
{
    public class HysteresisEdgeTrackingTest
    {
        [Fact]
        public void NonMaximumSuppressionNullTest()
        {
            try
            {
                var hysteresisEdgeTracking = new HysteresisEdgeTracking(null);
            }
            catch (NullReferenceException e)
            {
                Assert.True(true);
                return;
            }

            Assert.True(false);
        }

        [Fact]
        public void ProperHysteresisEdgeTrackingTest()
        {
            var inputData = new byte[,]
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

            var outputMatrix = new byte[,]
            {
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 255, 255, 0, 0, 255, 0 },
                { 0, 0, 0, 0, 255, 0, 255, 255, 0, 0 },
                { 0, 0, 0, 0, 255, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 255, 0, 0, 0, 255, 0 },
                { 0, 0, 0, 0, 255, 255, 255, 255, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 255, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
            };

            byte lowerBound = 100;
            byte upperBound = 200;
            var doubleThreshold = new DoubleThreshold(inputData, lowerBound, upperBound);
            var inputMatrix = doubleThreshold.CreateDoubleThresholdMatrix();
            HysteresisEdgeTracking hysteresisEdgeTracking = new HysteresisEdgeTracking(inputMatrix);
            var resultMatrix = hysteresisEdgeTracking.EdgeTracking();

            for (var i = 0; i < outputMatrix.GetLength(0); i++)
            {
                for (var j = 0; j < outputMatrix.GetLength(1); j++)
                {
                    if (outputMatrix[i, j] == resultMatrix[i,j]) continue;
                    Assert.True(false);
                    return;
                }
            }

            Assert.True(true);
        }
    }
}