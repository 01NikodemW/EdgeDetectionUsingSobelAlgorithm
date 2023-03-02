using System;

namespace SobelEdgeDetection
{
    public class DoubleThreshold
    {
        private readonly byte[,] _nmsArray;
        private readonly int _width;
        private readonly int _height;
        private readonly byte _lowerBound;
        private readonly byte _upperBound;

        public DoubleThreshold(byte[,] nmsArray, byte lowerBound, byte upperBound)
        {
            if (nmsArray == null)
            {
                throw new NullReferenceException("Input matrix cannot be null");
            }

            if (lowerBound == 0)
            {
                throw new ArgumentException("lowerBound cannot be zero");
            }


            if (lowerBound > upperBound)
            {
                throw new ArgumentException("lowerBound cannot be greater than upperbound");
            }

            _nmsArray = nmsArray;
            _width = nmsArray.GetLength(0);
            _height = nmsArray.GetLength(1);
            _lowerBound = lowerBound;
            _upperBound = upperBound;
        }


        public DoubleThresholdPixel[,] CreateDoubleThresholdMatrix()
        {
            var doubleThresholdMatrix = new DoubleThresholdPixel[_width, _height];

            for (var i = 0; i < _width; i++)
            {
                for (var j = 0; j < _height; j++)
                {
                    if (_nmsArray[i, j] < _lowerBound)
                    {
                        doubleThresholdMatrix[i, j] =
                            new DoubleThresholdPixel(DoubleThresholdPixelType.None);
                    }
                    else if (_nmsArray[i, j] < _upperBound)
                    {
                        doubleThresholdMatrix[i, j] =
                            new DoubleThresholdPixel(DoubleThresholdPixelType.Weak);
                    }
                    else
                    {
                        doubleThresholdMatrix[i, j] =
                            new DoubleThresholdPixel(DoubleThresholdPixelType.Strong);
                    }
                }
            }

            return doubleThresholdMatrix;
        }
    }
}