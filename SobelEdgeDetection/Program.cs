using System;
using System.CommandLine;
using System.Text.RegularExpressions;

namespace SobelEdgeDetection
{
    class Program
    {
        static void Main(string[] args)
        {
            var rootCommand = new RootCommand("Aplikacja wykrywająca krawędzie na wczytamym obrazie");
            var sigmaOpt = new Option<double>(new[] { "-s", "-sigma" }, "Ustaw wartość parametru sigma");
            sigmaOpt.SetDefaultValue(3.0);
            rootCommand.AddOption(sigmaOpt);

            var gaussMatrixSizeOpt = new Option<int>(new[] { "-g", "-gauss" }, "Ustaw szerokość macierzy Gaussa");
            gaussMatrixSizeOpt.SetDefaultValue(5);
            rootCommand.AddOption(gaussMatrixSizeOpt);

            var lowerBoundOpt = new Option<int>(new[] { "-l", "-lower" }, "Ustaw wartość granicy słabych pikseli");
            lowerBoundOpt.SetDefaultValue(60);
            rootCommand.AddOption(lowerBoundOpt);

            var upperBoundOpt = new Option<int>(new[] { "-u", "-upper" }, "Ustaw wartość granicy silnych pikseli");
            upperBoundOpt.SetDefaultValue(100);
            rootCommand.AddOption(upperBoundOpt);

            var inputArgument = new Argument<string>("input", "plik wejściowy");
            var outputArgument = new Argument<string>("output", "plik wyjściowy");
            rootCommand.AddArgument(inputArgument);
            rootCommand.AddArgument(outputArgument);
            rootCommand.SetHandler((string input, string output, double sigma, int gaussMatrixSize, int lowerBound,
                int upperBound) =>
            {
                try
                {
                    if (!Regex.Match(output,"[^\\s]+(.*?)\\.(jpg|jpeg|png)$").Success)
                    { 
                        throw new ArgumentException("Output file extension must be .jpg, .jpeg or .png");
                    }
                    if (lowerBound <1 || lowerBound > 255)
                    { 
                        throw new ArgumentException("Lowerbound has to be between 1-255");
                    }
                    if (upperBound <1 || upperBound > 255)
                    { 
                        throw new ArgumentException("UpperBound has to be between 1-255");
                    }
                    var enlargeMatrixPixels = (gaussMatrixSize - 1) / 2;
                    var image = new FImage(input);
                    image.EnlargeImage(enlargeMatrixPixels);
                    var gaussOperator = new GaussOperator(sigma, gaussMatrixSize, image.GetRed());
                    image.CopyMatrix(gaussOperator.MakeGaussOperator());
                    image.MiniaturizeImage(enlargeMatrixPixels);
                    image.EnlargeImage(1);
                    var sobelOperator = new SobelOperator(image.GetRed());
                    var nonMaximumSuppression = new NonMaximumSuppression(sobelOperator.MakeSobelOperator());
                    var doubleThreshold = new DoubleThreshold(nonMaximumSuppression.MakeNonMaximumSuppression(),
                        (byte)lowerBound, (byte)upperBound);
                    var hysteresisEdgeTracking =
                        new HysteresisEdgeTracking(doubleThreshold.CreateDoubleThresholdMatrix());
                    image.CopyMatrix(hysteresisEdgeTracking.EdgeTracking());
                    image.MiniaturizeImage(1);
                    image.ToImage()
                        .Save(output);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }, inputArgument, outputArgument, sigmaOpt, gaussMatrixSizeOpt, lowerBoundOpt, upperBoundOpt);

            rootCommand.Invoke(args);
        }
    }
}