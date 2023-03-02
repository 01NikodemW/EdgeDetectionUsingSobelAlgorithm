using System;

namespace SobelEdgeDetection
{
    public class HysteresisEdgeTracking
    {
        private readonly DoubleThresholdPixel[,] _doubleThresholdMatrix;
        private readonly int _width;
        private readonly int _height;

        public HysteresisEdgeTracking(DoubleThresholdPixel[,] dTMatrix)
        {
            if (dTMatrix == null)
            {
                throw new NullReferenceException("Input matrix cannot be null");
            }

            _doubleThresholdMatrix = dTMatrix;
            _width = _doubleThresholdMatrix.GetLength(0);
            _height = _doubleThresholdMatrix.GetLength(1);
        }

        public byte[,] EdgeTracking()
        {
            var nextOperation = true;
            while (nextOperation)
            {
                nextOperation = false;
                for (var i = 1; i < _width - 1; i++)
                {
                    for (var j = 1; j < _height - 1; j++)
                    {
                        if (_doubleThresholdMatrix[i, j].PixelType == DoubleThresholdPixelType.Strong)
                        {
                            for (var k = -1; k < 2; k++)
                            {
                                for (var l = -1; l < 2; l++)
                                {
                                    if (_doubleThresholdMatrix[i + k, j + l].PixelType == DoubleThresholdPixelType.Weak)
                                    {
                                        nextOperation = true;
                                        _doubleThresholdMatrix[i + k, j + l].PixelType =
                                            DoubleThresholdPixelType.Strong;
                                    }
                                }
                            }
                        }
                    }
                }
            }


            var outputArray = new byte[_width, _height];

            for (var i = 1; i < _width - 1; i++)
            {
                for (var j = 1; j < _height - 1; j++)
                {
                    if (_doubleThresholdMatrix[i, j].PixelType == DoubleThresholdPixelType.Strong)
                    {
                        outputArray[i, j] = 255;
                    }
                    else
                    {
                        outputArray[i, j] = 0;
                    }
                }
            }


            return outputArray;
        }
    }
}