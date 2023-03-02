using System;
using SobelEdgeDetection;
using Xunit;

namespace SobelEdgeDetectionTests
{
    public class NonMaximumSuppressionTest
    {
        [Fact]
        public void NonMaximumSuppressionNullTest()
        {
            try
            {
                var nonMaximumSuppression = new NonMaximumSuppression(null);
            }
            catch (NullReferenceException e)
            {
                Assert.True(true);
                return;
            }

            Assert.True(false);
        }


        [Theory]
        [InlineData(70, 0)]
        [InlineData(20, 140)]
        [InlineData(165, 140)]
        [InlineData(40, 0)]
        [InlineData(130, 0)]
        public void DirectionTest(int angle, int value)
        {
            var gradientMatrix = CreateGradientMatrix();
            var angleMatrix = CreateAngleMatrix(angle);
            var sobelPixelMatrix = CreateSobelPixelMatrix(gradientMatrix, angleMatrix);

            var nonMaximumSuppression = new NonMaximumSuppression(sobelPixelMatrix);
            var output = nonMaximumSuppression.MakeNonMaximumSuppression();
            var result = output[1, 1] == value;
            Assert.True(result);
        }

        [Fact]
        public void NonMaximumSuppressionCalculationTest()
        {
            var gradientMatrix = new byte[,]
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

            var angleMatrix = new[,]
            {
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                {
                    0, 90, 88.97696981133217, 77.94921511659045, 77.38067770656923, 63.778033222445536,
                    177.87890360333856, 41.5526131481348, 54.5600964808863, 0
                },
                {
                    0, 89.54164354199956, 87.67218491095885, 76.30412349559099, 75.25643716352927, 60.94539590092286,
                    5.599339336520571, 34.99202019855866, 48.447386851865204, 0
                },
                {
                    0, 85.98582430458899, 89.13194855025446, 83.49104355949743, 82.1309244482095, 78.00310069207643,
                    16.69924423399362, 113.96248897457819, 146.6893691754392, 0
                },
                {
                    0, 83.6598082540901, 99.46232220802563, 105.25511870305779, 106.92751306414705, 131.74277805933306,
                    6.900328169277287, 2.121096396661443, 179.48383577023517, 0
                },
                {
                    0, 81.2538377374448, 120.96375653207352, 130.36453657309735, 130.6012946450045, 158.2945632538944,
                    8.481605794029349, 4.159642293712636, 1.9556813975228806, 0
                },
                {
                    0, 45, 169.9920201985587, 148.1340223063963, 138.94518622903757, 166.96134341697703,
                    10.039254787152345, 2.1610794882263633, 1.8307486480254909, 0
                },
                {
                    0, 93.17983011986423, 49.39870535499551, 164.1342921972262, 141.00900595749454, 177.13759477388825,
                    14.95008637901256, 4.853096386992377, 8.196111287524673, 0
                },
                {
                    0, 85.10090754621223, 69.92847413546127, 174.28940686250036, 126.76438067579049, 14.588918732874646,
                    37.87498365109818, 20.409882833803977, 34.530825742288584, 0
                },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
            };

            var outputMatrix = new byte[,]
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

            var sobelPixelMatrix = CreateSobelPixelMatrix(gradientMatrix, angleMatrix);
            var nonMaximumSuppression = new NonMaximumSuppression(sobelPixelMatrix);
            var resultMatrix = nonMaximumSuppression.MakeNonMaximumSuppression();
            Assert.Equal(outputMatrix, resultMatrix);
        }

        private SobelPixel[,] CreateSobelPixelMatrix(byte[,] gradientMatrix, Double[,] angleMatrix)
        {
            var sobelPixelMatrix = new SobelPixel[gradientMatrix.GetLength(0), gradientMatrix.GetLength(1)];
            for (var i = 0; i < gradientMatrix.GetLength(0); i++)
            {
                for (var j = 0; j < gradientMatrix.GetLength(1); j++)
                {
                    sobelPixelMatrix[i, j] = new SobelPixel(gradientMatrix[i, j], angleMatrix[i, j]);
                }
            }

            return sobelPixelMatrix;
        }

        private byte[,] CreateGradientMatrix()
        {
            var gradientMatrix = new byte[,]
            {
                { 152, 138, 118 },
                { 151, 140, 112 },
                { 151, 125, 123 },
            };
            return gradientMatrix;
        }

        private double[,] CreateAngleMatrix(double angle)
        {
            var angleMatrix = new[,]
            {
                { 0, 0, 0 },
                { 0, angle, 0 },
                { 0, 0, 0 },
            };
            return angleMatrix;
        }
    }
}