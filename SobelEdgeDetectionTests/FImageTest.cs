using System;
using System.IO;
using SobelEdgeDetection;
using Xunit;

namespace SobelEdgeDetectionTests
{
    public class FImageTest
    {
        [Fact]
        public void FImageNullTest()
        {
            try
            {
                var image = new FImage(null);
            }
            catch (NullReferenceException e)
            {
                Assert.True(true);
                return;
            }

            Assert.True(false);
        }
        
        [Fact]
        public void WrongExtenstionTest()
        {
            try
            {
                var image = new FImage(@"../../../zdjtest.wrong");
            }
            catch (ArgumentException e)
            {
                Assert.True(true);
                return;
            }

            Assert.True(false);
        }
        
        [Fact]
        public void NotExistingFileTest()
        {
            try
            {
                if(File.Exists(@"../../../notExistingFile.jpg")) File.Delete(@"../../../notExistingFile.jpg");
                var image = new FImage(@"../../../notExistingFile.jpg");
            }
            catch (ArgumentException e)
            {
                Assert.True(true);
                return;
            }

            Assert.True(false);
        }


        [Fact]
        public void ImageEnlargeTest()
        {
            var image = new FImage(@"../../../zdjtest.jpeg");
            //   var image = new FImage(@"D:\Jusersy\Nikodem\Desktop\ProjektZTestami2\SobelEdgeDetection\SobelEdgeDetection\zdjtest.jpeg");
            var size0 = image.GetRed().GetLength(0);
            var size1 = image.GetRed().GetLength(1);
            image.EnlargeImage(1);
            Assert.Equal(size0 + 2, image.GetRed().GetLength(0));
            Assert.Equal(size1 + 2, image.GetRed().GetLength(1));
        }

        [Fact]
        public void ImageMiniaturizeTest()
        {
            var image = new FImage(@"../../../zdjtest.jpeg");
            var size0 = image.GetRed().GetLength(0);
            var size1 = image.GetRed().GetLength(1);
            image.MiniaturizeImage(1);
            Assert.Equal(size0 - 2, image.GetRed().GetLength(0));
            Assert.Equal(size1 - 2, image.GetRed().GetLength(1));
        }

        [Fact]
        public void ImageMiniaturizeTooSmallTest()
        {
            var image = new FImage(@"../../../zdjtest.jpeg");
            var size0 = image.GetRed().GetLength(0);
            var size1 = image.GetRed().GetLength(1);
            try
            {
                image.MiniaturizeImage(size0);
            }
            catch (Exception e)
            {
                Assert.True(true);
                return;
            }

            Assert.True(false);
        }

        [Fact]
        public void GetRedTest()
        {
            var image = new FImage(@"../../../zdjtest.jpeg");
            

            var expectedMatrix = new byte[,]
            {
                { 129, 160, 223, 53, 5, 139, 113, 40, 76, 47 },
                { 130, 150, 228, 43, 15, 157, 132, 20, 41, 28 },
                { 130, 148, 243, 49, 10, 165, 27, 6, 37, 30 },
                { 123, 142, 242, 40, 40, 63, 22, 18, 44, 14 },
                { 124, 162, 234, 56, 1, 34, 22, 13, 16, 1 },
                { 107, 172, 221, 76, 5, 6, 10, 16, 11, 48 },
                { 96, 165, 217, 60, 62, 95, 123, 169, 204, 242 },
                { 85, 185, 218, 68, 196, 234, 227, 229, 174, 101 },
                { 88, 179, 189, 73, 232, 226, 134, 76, 77, 110 },
                { 85, 199, 189, 79, 201, 113, 68, 125, 218, 252 }
            };

            Assert.Equal(expectedMatrix, image.GetRed());
        }


        [Fact]
        public void CopyFinalMatrixTest()
        {
            var image = new FImage(@"../../../zdjtest.jpeg");
            var matrixToCopy = new byte[,]
            {
                { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 },
                { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 },
                { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 },
                { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 },
                { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 },
                { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 },
                { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 },
                { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 },
                { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 },
                { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 },
            };

            image.CopyMatrix(matrixToCopy);
            Assert.Equal(matrixToCopy, image.GetRed());
        }
    }
}