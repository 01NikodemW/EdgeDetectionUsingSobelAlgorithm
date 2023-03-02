using System;
using SobelEdgeDetection;
using Xunit;

namespace SobelEdgeDetectionTests
{
    public class GaussOperatorTest
    {
        [Fact]
        public void GaussOperatorSizeEvenTest()
        {
            var testMatrix = new byte[,]
            {
                { 123, 255 },
                { 221, 255 },
            };

            try
            {
                var gaussOperator = new GaussOperator(3, 2, testMatrix);
            }
            catch (ArgumentException e)
            {
                Assert.True(true);
                return;
            }

            Assert.True(false);
        }

        [Fact]
        public void GaussOperatorWrongSigmaTest()
        {
            var testMatrix = new byte[,]
            {
                { 123, 255 },
                { 221, 255 },
            };

            try
            {
                var gaussOperator = new GaussOperator(0, 2, testMatrix);
            }
            catch (ArgumentException e)
            {
                Assert.True(true);
                return;
            }

            Assert.True(false);
        }

        [Fact]
        public void GaussOperatorSizeLowerThanOneTest()
        {
            var testMatrix = new byte[,]
            {
                { 123, 255 },
                { 221, 255 },
            };

            try
            {
                var gaussOperator = new GaussOperator(3, 0, testMatrix);
            }
            catch (ArgumentException e)
            {
                Assert.True(true);
                return;
            }

            Assert.True(false);
        }

        [Fact]
        public void GaussOperatorNullTest()
        {
            try
            {
                var gaussOperator = new GaussOperator(3, 3, null);
            }
            catch (NullReferenceException e)
            {
                Assert.True(true);
                return;
            }

            Assert.True(false);
        }

        [Fact]
        public void GaussOperatorCalculationTest()
        {
            var inputMatrix = new byte[,]
            {
                { 129, 129, 160, 223, 53, 5, 139, 113, 40, 76, 47, 47 },
                { 129, 129, 160, 223, 53, 5, 139, 113, 40, 76, 47, 47 },
                { 130, 130, 150, 228, 43, 15, 157, 132, 20, 41, 28, 28 },
                { 130, 130, 148, 243, 49, 10, 165, 27, 6, 37, 30, 30 },
                { 123, 123, 142, 242, 40, 40, 63, 22, 18, 44, 14, 14 },
                { 124, 124, 162, 234, 56, 1, 34, 22, 13, 16, 1, 1 },
                { 107, 107, 172, 221, 76, 5, 6, 10, 16, 11, 48, 48 },
                { 96, 96, 165, 217, 60, 62, 95, 123, 169, 204, 242, 242 },
                { 85, 85, 185, 218, 68, 196, 234, 227, 229, 174, 101, 101 },
                { 88, 88, 179, 189, 73, 232, 226, 134, 76, 77, 110, 110 },
                { 85, 85, 199, 189, 79, 201, 113, 68, 125, 218, 252, 252 },
                { 85, 85, 199, 189, 79, 201, 113, 68, 125, 218, 252, 252 }
            };

            var outputMatrix = new byte[,]
            {
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 138, 169, 145, 93, 66, 91, 99, 71, 46, 48, 0 },
                { 0, 137, 170, 145, 95, 69, 86, 89, 54, 36, 40, 0 },
                { 0, 133, 170, 144, 100, 64, 71, 67, 37, 26, 29, 0 },
                { 0, 133, 171, 147, 100, 50, 43, 40, 22, 20, 20, 0 },
                { 0, 131, 169, 151, 100, 35, 22, 22, 19, 19, 21, 0 },
                { 0, 127, 166, 152, 102, 43, 39, 53, 63, 78, 89, 0 },
                { 0, 121, 163, 154, 123, 88, 106, 123, 130, 133, 131, 0 },
                { 0, 118, 158, 151, 144, 139, 171, 169, 158, 153, 150, 0 },
                { 0, 119, 157, 153, 159, 159, 181, 158, 146, 150, 153, 0 },
                { 0, 120, 156, 153, 157, 147, 150, 115, 123, 162, 194, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
            };

            var sigma = 3;
            var gaussMatrixSize = 3;

            GaussOperator gaussOperator = new GaussOperator(sigma, gaussMatrixSize, inputMatrix);
            var resultMatrix = gaussOperator.MakeGaussOperator();

            Assert.Equal(outputMatrix, resultMatrix);
        }
    }
}