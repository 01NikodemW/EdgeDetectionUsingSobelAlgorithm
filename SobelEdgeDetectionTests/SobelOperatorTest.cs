using System;
using SobelEdgeDetection;
using Xunit;

namespace SobelEdgeDetectionTests
{
    public class SobelOperatorTest
    {
        [Fact]
        public void SobelOperatorNullTest()
        {
            try
            {
                var sobelOperator = new SobelOperator(null);
            }
            catch (NullReferenceException e)
            {
                Assert.True(true);
                return;
            }

            Assert.True(false);
        }


        [Fact]
        public void SobelOperatorCalculationsTest()
        {
            var testMatrix = new byte[,]
            {
                { 123, 255, 134 },
                { 221, 255, 231 },
                { 121, 23, 142 }
            };

            var sobelOperator = new SobelOperator(testMatrix);
            var resultMatrix = sobelOperator.MakeSobelOperator();
            var gx = testMatrix[0, 0] + 2 * testMatrix[1, 0] + testMatrix[2, 0] - testMatrix[0, 2] -
                     2 * testMatrix[1, 2] - testMatrix[2, 2];
            var gy = testMatrix[0, 0] + 2 * testMatrix[0, 1] + testMatrix[0, 2] - testMatrix[2, 0] -
                     2 * testMatrix[2, 1] - testMatrix[2, 2];

            var angle = Math.Atan2(gx, gy) * (180 / Math.PI);
            if (angle < 0) angle = angle + 180;

            Assert.Equal((int)Math.Sqrt(gx * gx + gy * gy), resultMatrix[1, 1].Gradient);
            Assert.Equal(angle, resultMatrix[1, 1].Angle);
        }

        [Fact]
        public void SobelOperatorTenXTenMatrixTest()
        {
            var inputMatrix = new byte[,]
            {
                { 152, 141, 118, 114, 102, 70, 72, 74, 53, 42 },
                { 151, 140, 119, 113, 97, 63, 64, 64, 44, 36 },
                { 151, 141, 119, 110, 90, 53, 52, 50, 33, 27 },
                { 149, 140, 120, 106, 81, 42, 37, 37, 25, 23 },
                { 145, 139, 121, 106, 79, 45, 45, 52, 52, 57 },
                { 140, 136, 127, 116, 96, 72, 78, 82, 83, 84 },
                { 135, 134, 133, 132, 117, 99, 105, 106, 104, 104 },
                { 130, 131, 140, 146, 134, 121, 132, 134, 137, 143 },
                { 128, 130, 146, 158, 147, 139, 155, 157, 164, 176 },
                { 127, 130, 151, 163, 149, 139, 154, 153, 163, 181 }
            };

            var expectedMatrix = new byte[,]
            {
                { 152, 141, 118, 114, 102, 70, 72, 74, 53, 42 },
                { 151, 130, 112, 91, 205, 149, 81, 117, 136, 36 },
                { 151, 125, 123, 122, 235, 175, 102, 122, 117, 27 },
                { 149, 114, 132, 149, 248, 163, 20, 39, 83, 23 },
                { 145, 90, 121, 159, 240, 174, 158, 189, 222, 57 },
                { 140, 52, 87, 157, 239, 221, 230, 220, 205, 84 },
                { 135, 9, 51, 130, 205, 195, 212, 212, 219, 104 },
                { 130, 36, 73, 98, 162, 160, 197, 212, 245, 143 },
                { 128, 70, 110, 60, 108, 75, 102, 91, 132, 176 },
                { 127, 130, 151, 163, 149, 139, 154, 153, 163, 181 }
            };

            var sobelOperator = new SobelOperator(inputMatrix);
            var outputMatrix = sobelOperator.MakeSobelOperator();

            for (var i = 0; i < expectedMatrix.GetLength(0); i++)
            {
                for (var j = 0; j < expectedMatrix.GetLength(1); j++)
                {
                    if (expectedMatrix[i, j] != outputMatrix[i, j].Gradient)
                    {
                        Assert.True(false);
                        return;
                    }

                    if (outputMatrix[i, j].Angle < 0 || outputMatrix[i, j].Angle > 180)
                    {
                        Assert.True(false);
                        return;
                    }
                }
            }

            Assert.True(true);
        }
    }
}