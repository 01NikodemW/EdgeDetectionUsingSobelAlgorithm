using System;

namespace SobelEdgeDetection
{
    public class GaussOperator
    {
        private readonly double _sigma;
        private readonly int _size;
        private readonly double[,] _gaussianKernel;
        private readonly byte[,] _red;
        private readonly int _width;
        private readonly int _height;

        public GaussOperator(double sigma, int size, byte[,] red)
        {
            if (red == null)
            {
                throw new NullReferenceException("Input matrix cannot be null");
            }


            if (size < 1) throw new ArgumentException("Siza cannot be lower than 1");
            if ((size - 1) % 2 != 0) throw new ArgumentException("Siza must be odd number");
            if (sigma <= 0) throw new ArgumentException("Value of sigma must be positive number");


            _red = red;
            _width = red.GetLength(0);
            _height = red.GetLength(1);
            _sigma = sigma;
            _size = size;
            _gaussianKernel = CreateGaussianKernel();
        }


        public byte[,] MakeGaussOperator()
        {
            var matrixAfterGauss = new byte[_width, _height];

            var mid = (_size - 1) / 2;

            for (var i = mid; i < _width - mid; i++)
            {
                for (var j = mid; j < _height - mid; j++)
                {
                    var blurValue = GetGaussValue(_gaussianKernel, i, j, _size);
                    matrixAfterGauss[i, j] = (byte)blurValue;
                }
            }


            return matrixAfterGauss;
        }

        private double GetGaussValue(double[,] kernel, int x, int y, int size)
        {
            var mid = (size - 1) / 2;
            double sum = 0;

            for (var i = -mid; i <= mid; i++)
            {
                for (var j = -mid; j <= mid; j++)
                {
                    sum = sum + kernel[i + mid, j + mid] * _red[x + i, y + j];
                }
            }

            if (sum > 255) sum = 255;
            if (sum < 0) sum = 0;


            return sum;
        }


        private double[,] CreateGaussianKernel()
        {
            var kernel = new double[_size, _size];

            var mid = (_size - 1) / 2;
            double sum = 0;
            for (var i = -mid; i <= mid; i++)
            {
                for (var j = -mid; j <= mid; j++)
                {
                    kernel[i + mid, j + mid] = CalculateGaussianValue(_sigma, i, j);
                    sum = sum + kernel[i + mid, j + mid];
                }
            }

            var ratio = 1 / sum;


            for (var i = -mid; i <= mid; i++)
            {
                for (var j = -mid; j <= mid; j++)
                {
                    kernel[i + mid, j + mid] = kernel[i + mid, j + mid] * ratio;
                }
            }

            return kernel;
        }

        private double CalculateGaussianValue(double sigma, int x, int y)
        {
            return (1 / (2 * Math.PI * sigma * sigma)) * Math.Exp(-(x * x + y * y) / (2 * sigma * sigma));
        }
    }
}