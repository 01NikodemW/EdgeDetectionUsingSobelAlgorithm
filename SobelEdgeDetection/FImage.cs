using System;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;

namespace SobelEdgeDetection
{
    public class FImage
    {
        private byte[,] _red;
        private byte[,] _green;
        private byte[,] _blue;

        private int _width;
        private int _height;

        public FImage(String path)
        {
            if (path == null)
            {
                throw new NullReferenceException("Input matrix cannot be null");
            }

            if (!Regex.Match(path, "[^\\s]+(.*?)\\.(jpg|jpeg|png)$").Success)
            {
                throw new ArgumentException("Input file extension must be .jpg, .jpeg or .png");
            }

            if (!File.Exists(path))
            {
                throw new ArgumentException("Selected file does not exists");
            }

            var image = Image.FromFile(path);
            this._width = image.Width;
            this._height = image.Height;

            if (_width < 3) throw new ArgumentException("Size cannot be lower than 3");
            if (_height < 3) throw new ArgumentException("Size cannot be lower than 3");

            _red = new byte[_width, _height];
            _green = new byte[_width, _height];
            _blue = new byte[_width, _height];

            var bitmap = new Bitmap(image);
            for (var i = 0; i < _width; i++)
            {
                for (var j = 0; j < _height; j++)
                {
                    var color = bitmap.GetPixel(i, j);
                    _red[i, j] = color.R;
                    _green[i, j] = color.G;
                    _blue[i, j] = color.B;
                }
            }

            ToGrayScale();
        }

        public byte[,] GetRed()
        {
            return _red;
        }


        public Image ToImage()
        {
            var bitmap = new Bitmap(_width, _height);
            for (var i = 0; i < _width; i++)
            {
                for (var j = 0; j < _height; j++)
                {
                    var color = Color.FromArgb(_red[i, j], _red[i, j], _red[i, j]);
                    bitmap.SetPixel(i, j, color);
                }
            }

            return bitmap;
        }


        public void CopyMatrix(byte[,] finalArray)
        {
            CopyMatrix(finalArray, ref _red);
        }

        public void EnlargeImage(int howManyPixels)
        {
            for (var i = 0; i < howManyPixels; i++)
            {
                Enlarge();
            }
        }

        public void MiniaturizeImage(int howManyPixels)
        {
            for (var i = 0; i < howManyPixels; i++)
            {
                if (_red.GetLength(0) < 3) throw new Exception("Image is to small to miniaturize");
                Reduce();
            }
        }

        private void Enlarge()
        {
            var enlargedImage = new byte[_width + 2, _height + 2];

            for (var i = 0; i < _width; i++)
            {
                for (var j = 0; j < _height; j++)
                {
                    enlargedImage[i + 1, j + 1] = _red[i, j];
                }
            }

            for (var i = 1; i < _width + 1; i++)
            {
                enlargedImage[i, 0] = _red[i - 1, 0];
                enlargedImage[i, _height + 1] = _red[i - 1, _height - 1];
            }

            for (var j = 1; j < _height + 1; j++)
            {
                enlargedImage[0, j] = _red[0, j - 1];
                enlargedImage[_width + 1, j] = _red[_width - 1, j - 1];
            }

            enlargedImage[0, 0] = _red[0, 0];
            enlargedImage[_width + 1, 0] = _red[_width - 1, 0];
            enlargedImage[0, _height + 1] = _red[0, _height - 1];
            enlargedImage[_width + 1, _height + 1] = _red[_width - 1, _height - 1];


            _red = new byte[_width + 2, _height + 2];
            CopyMatrix(enlargedImage, ref _red);

            _width = _width + 2;
            _height = _height + 2;
        }


        private void Reduce()
        {
            var reducedImage = new byte[_width - 2, _height - 2];

            for (var i = 0; i < _width - 2; i++)
            {
                for (var j = 0; j < _height - 2; j++)
                {
                    reducedImage[i, j] = _red[i + 1, j + 1];
                }
            }

            _red = new byte[_width - 2, _height - 2];
            CopyMatrix(reducedImage, ref _red);

            _width = _width - 2;
            _height = _height - 2;
        }

        private void ToGrayScale()
        {
            var grayscaleMatrix = new byte[_width, _height];
            for (var i = 0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
                {
                    grayscaleMatrix[i, j] = (byte)(0.299 * _red[i, j] + 0.587 * _green[i, j] + 0.114 * _blue[i, j]);
                }
            }

            CopyMatrix(grayscaleMatrix, ref _red);
            CopyMatrix(grayscaleMatrix, ref _green);
            CopyMatrix(grayscaleMatrix, ref _blue);
        }

        private void CopyMatrix(byte[,] srcArray, ref byte[,] dstArray)
        {
            for (var i = 0; i < srcArray.GetLength(0); i++)
            {
                for (var j = 0; j < srcArray.GetLength(1); j++)
                {
                    dstArray[i, j] = srcArray[i, j];
                }
            }
        }
    }
}