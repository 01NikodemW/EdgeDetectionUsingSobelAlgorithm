namespace SobelEdgeDetection
{
    public class DoubleThresholdPixel
    {
        private DoubleThresholdPixelType _pixelType;

        public DoubleThresholdPixel( DoubleThresholdPixelType pType)
        {
            _pixelType = pType;
        }

        public DoubleThresholdPixelType PixelType
        {
            get { return _pixelType; }
            set { _pixelType = value; }
        }
    }
}