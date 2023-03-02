namespace SobelEdgeDetection
{
    public class SobelPixel
    {
        private readonly int _gradient;
        private readonly double _angle;

        public SobelPixel(int gradient, double angle)
        {
            _gradient = gradient;
            _angle = angle;
        }

        public int Gradient
        {
            get { return _gradient; }
        }

        public double Angle
        {
            get { return _angle; }
        }
    }
}