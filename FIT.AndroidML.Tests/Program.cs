using FIT.AndroidML.MLCore;
using System;

namespace FIT.AndroidML.Tests
{
  class Program
  {
    static void Main(string[] args)
    {

      string location = @"D:\machinelearning-samples-master\samples\csharp\getting-started\DeepLearning_ImageClassification_Training\ImageClassification.Train\assets\inputs\images\flower_photos_small_set\sunflowers\6953297_8576bf4ea3.jpg";
      MLPredictionEngine engine = new MLPredictionEngine();


      for(; ; )
      {
        Console.WriteLine("");
        Console.WriteLine("");
        string input = Console.ReadLine();

        try
        {
          var result = engine.Predict(input);

          Console.WriteLine($"{result.Score[0]} - {result.Score[1]} - {result.PredictedLabel} - {result.Seconds}s");

        }
        catch(Exception e)
        {
          Console.WriteLine("Exception " + e);
        }

      }


    }
  }
}
