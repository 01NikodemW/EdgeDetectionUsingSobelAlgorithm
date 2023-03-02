using System;

namespace SobelEdgeDetection
{
    public class SobelOperator
    {
        private readonly byte[,] _pixels;
        private readonly int _width;
        private readonly int _height;

        public SobelOperator(byte[,] pixels)
        {
            if (pixels == null)
            {
                throw new NullReferenceException("Input matrix cannot be null");
            }

            _pixels = pixels;
            _width = pixels.GetLength(0);
            _height = pixels.GetLength(1);
        }

        public SobelPixel[,] MakeSobelOperator()
        {
            var sobelOperatorMatrix = new SobelPixel[_width, _height];
            for (var i = 0; i < _width; i++)
            {
                for (var j = 0; j < _height; j++)
                {
                    sobelOperatorMatrix[i, j] = new SobelPixel(_pixels[i, j], 0);
                }
            }

            for (var i = 1; i < _width - 1; i++)
            {
                for (var j = 1; j < _height - 1; j++)
                {
                    var gx = ValueGx(i, j, _pixels);
                    var gy = ValueGy(i, j, _pixels);

                    var angle = Math.Atan2(gy, gx) * (180 / Math.PI);
                    if (angle < 0) angle = angle + 180;

                    var gradient = (int)Math.Sqrt(gx * gx + gy * gy);
                    sobelOperatorMatrix[i, j] = new SobelPixel(gradient, angle);
                }
            }


            return sobelOperatorMatrix;
        }


        private double ValueGx(int i, int j, byte[,] pixels)
        {
            double sum = 0;
            for (var k = 0; k < 3; k++)
            {
                for (var l = 0; l < 3; l++)
                {
                    sum += _sobelGxKernel[k, l] * pixels[i - 1 + l, j - 1 + k];
                }
            }

            return sum;
        }

        private double ValueGy(int i, int j, byte[,] pixels)
        {
            double sum = 0;
            for (var k = 0; k < 3; k++)
            {
                for (var l = 0; l < 3; l++)
                {
                    sum += _sobelGyKernel[k, l] * pixels[i - 1 + l, j - 1 + k];
                }
            }

            return sum;
        }

        private readonly double[,] _sobelGxKernel = new double[,]
        {
            { 1, 0, -1 },
            { 2, 0, -2 },
            { 1, 0, -1 }
        };

        private readonly double[,] _sobelGyKernel = new double[,]
        {
            { 1, 2, 1 },
            { 0, 0, 0 },
            { -1, -2, -1 }
        };
    }
}