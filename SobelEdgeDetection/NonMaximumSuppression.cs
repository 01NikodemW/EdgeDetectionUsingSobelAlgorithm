using System;

namespace SobelEdgeDetection
{
    public class NonMaximumSuppression
    {
        private readonly SobelPixel[,] _sobelOperatorMatrix;
        private readonly int _width;
        private readonly int _height;

        public NonMaximumSuppression(SobelPixel[,] sobelOperatorMatrix)
        {
            if (sobelOperatorMatrix == null)
            {
                throw new NullReferenceException("Input matrix cannot be null");
            }

            _sobelOperatorMatrix = sobelOperatorMatrix;
            _width = sobelOperatorMatrix.GetLength(0);
            _height = sobelOperatorMatrix.GetLength(1);
        }

        public byte[,] MakeNonMaximumSuppression()
        {
            var nmsMatrix = new byte[_width, _height];
            for (var i = 1; i < _width - 1; i++)
            {
                for (var j = 1; j < _height - 1; j++)
                {
                    double value1 = 255;
                    double value2 = 255;

                    if ((_sobelOperatorMatrix[i, j].Angle >= 0 && _sobelOperatorMatrix[i, j].Angle < 22.5) ||
                        (_sobelOperatorMatrix[i, j].Angle >= 157.5 && _sobelOperatorMatrix[i, j].Angle <= 180))
                    {
                        value1 = _sobelOperatorMatrix[i - 1, j].Gradient;
                        value2 = _sobelOperatorMatrix[i + 1, j].Gradient;
                    }
                    else if (_sobelOperatorMatrix[i, j].Angle >= 22.5 && _sobelOperatorMatrix[i, j].Angle < 67.5)
                    {
                        value1 = _sobelOperatorMatrix[i - 1, j - 1].Gradient;
                        value2 = _sobelOperatorMatrix[i + 1, j + 1].Gradient;
                    }
                    else if (_sobelOperatorMatrix[i, j].Angle >= 67.5 && _sobelOperatorMatrix[i, j].Angle < 112.5)
                    {
                        value1 = _sobelOperatorMatrix[i, j + 1].Gradient;
                        value2 = _sobelOperatorMatrix[i, j - 1].Gradient;
                    }
                    else if (_sobelOperatorMatrix[i, j].Angle >= 112.5 && _sobelOperatorMatrix[i, j].Angle < 157.5)
                    {
                        value1 = _sobelOperatorMatrix[i - 1, j + 1].Gradient;
                        value2 = _sobelOperatorMatrix[i + 1, j - 1].Gradient;
                    }


                    if (_sobelOperatorMatrix[i, j].Gradient > value1 && _sobelOperatorMatrix[i, j].Gradient > value2)
                    {
                        if (_sobelOperatorMatrix[i, j].Gradient > 255)
                        {
                            nmsMatrix[i, j] = 255;
                        }
                        else
                        {
                            nmsMatrix[i, j] = (byte)_sobelOperatorMatrix[i, j].Gradient;
                        }
                    }
                    else
                    {
                        nmsMatrix[i, j] = 0;
                    }
                }
            }

            return nmsMatrix;
        }
    }
}