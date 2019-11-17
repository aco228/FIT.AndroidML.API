using FIT.Android.Models.ML;
using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FIT.AndroidML.MLCore
{
  public class MLPredictionEngine : MLBase
  {
    protected DataViewSchema Schema;
    protected ITransformer Transformer;
    protected PredictionEngine<InMemoryImageData, ImagePrediction> PredictionEngine;

    public MLPredictionEngine()
    {
      Console.WriteLine("MLPredictionEngine starting");
      DateTime created = new DateTime();

      this.Transformer = this.Context.Model.Load(MLFileManager.ModelLocation, out this.Schema);
      this.PredictionEngine = this.Context.Model.CreatePredictionEngine<InMemoryImageData, ImagePrediction>(this.Transformer);

      Console.WriteLine("Ready..");
    }

    public ImagePrediction Predict(string location)
    {
      FileInfo file = new FileInfo(location);
      if (!file.Exists)
        throw new Exception("No file");

      if (!file.Extension.Equals(".jpg") && !file.Extension.Equals(".png"))
        throw new Exception("Only jpg and png");

      var data = new InMemoryImageData(
        image: File.ReadAllBytes(location),
        label: "",
        imageFileName: Path.GetFileName(location));

      return this.Predict(data);
    }

    public ImagePrediction Predict(InMemoryImageData data)
    {
      DateTime started = DateTime.Now;
      var result = this.PredictionEngine.Predict(data);
      result.Seconds = (int)(started - DateTime.Now).TotalSeconds;

      return result;
    }


  }
}
