using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using FIT.AndroidML.Models.ML;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms;
using Microsoft.ML.Vision;
using static Microsoft.ML.Transforms.ValueToKeyMappingEstimator;

namespace FIT.AndroidML.MLCore
{
  public class MLTrainer : MLBase
  {
    IEnumerable<ImageData> _inputData = null;
    IDataView fullImagesDataset;
    IDataView shuffledFullImageFilePathsDataset;
    IDataView shuffledFullImagesDataset;
    IDataView trainDataView;
    IDataView testDataView;
    ITransformer trainedModel;
    EstimatorChain<KeyToValueMappingTransformer> pipeline;


    public MLTrainer()
    {
      this._inputData = this.GetTrainingImagesModels(folder: MLFileManager.InputFolder);
    }

    private IEnumerable<ImageData> GetTrainingImagesModels(string folder, bool useFolderNameAsLabel = true)
      => FileUtils.LoadImagesFromDirectory(folder, useFolderNameAsLabel).Select(x => new ImageData(x.imagePath, x.label));

    //
    //  Preparation 
    //

    // 1
    public void LoadIntoMemory()
    {
      Console.WriteLine("[ML.Trainer] Loading data into memory");
      foreach (var entry in this._inputData)
        Console.WriteLine($"\t {entry.Label} - {entry.ImagePath} ");
      fullImagesDataset = this.Context.Data.LoadFromEnumerable(this._inputData);
      shuffledFullImageFilePathsDataset = this.Context.Data.ShuffleRows(fullImagesDataset);
    }

    // 2
    public void TransformDataByLabels()
    {
      Console.WriteLine("[ML.Trainer] Data transformation by labels ");
      shuffledFullImagesDataset = this.Context.Transforms.Conversion.
        MapValueToKey(outputColumnName: "LabelAsKey", inputColumnName: "Label", keyOrdinality: KeyOrdinality.ByValue)
        .Append(this.Context.Transforms.LoadRawImageBytes(
          outputColumnName: "Image",
          imageFolder: MLFileManager.InputFolder,
          inputColumnName: "ImagePath"))
        .Fit(shuffledFullImageFilePathsDataset)
        .Transform(shuffledFullImageFilePathsDataset);

      // we wont need it anymore
      fullImagesDataset = null;
      shuffledFullImageFilePathsDataset = null;
    }

    // 3
    public void GenerateTrainTestDataAndInvokePipeline()
    {
      Console.WriteLine("[ML.Trainer] Generating TrainSplitTest and constructing Pipeline... ");
      var trainTestData = this.Context.Data.TrainTestSplit(shuffledFullImagesDataset, testFraction: 0.8);
      trainDataView = trainTestData.TrainSet;
      testDataView = trainTestData.TestSet;

      pipeline = this.Context.MulticlassClassification.Trainers 
        .ImageClassification(featureColumnName: "Image",
          labelColumnName: "LabelAsKey",
          validationSet: testDataView)
        .Append(this.Context.Transforms.Conversion.MapKeyToValue(
          outputColumnName: "PredictedLabel",
          inputColumnName: "PredictedLabel"));

      shuffledFullImagesDataset = null;
    }


    //
    //  Training
    //

    public void StartTraining()
    {
      Console.WriteLine(" ");
      Console.WriteLine("[ML.Trainer] Starting training... ");

      DateTime started = DateTime.Now;
      trainedModel = pipeline.Fit(trainDataView);


      Console.WriteLine($"[ML.Trainer] Training ended in {(DateTime.Now - started).TotalSeconds} seconds");
      Console.WriteLine(" ");
      Console.WriteLine(" ");

    }

    public void Evaluation()
    {
      Console.WriteLine("[ML.Trainer]. Starting evaluation process");

      // Measuring time
      var watch = Stopwatch.StartNew();

      var predictionsDataView = trainedModel.Transform(testDataView);

      var metrics = this.Context.MulticlassClassification.Evaluate(predictionsDataView, labelColumnName: "LabelAsKey", predictedLabelColumnName: "PredictedLabel");
      this.PrintMultiClassClassificationMetrics("Evaluation", metrics);

      watch.Stop();
      var elapsed2Ms = watch.ElapsedMilliseconds;

      Console.WriteLine($"[ML.Trainer]. Predicting and Evaluation took: {elapsed2Ms / 1000} seconds");

    }

    public void Save()
    {
      this.Context.Model.Save(trainedModel, trainDataView.Schema, MLFileManager.ModelLocation);
      Console.WriteLine($"Model saved to: {MLFileManager.ModelLocation}");
    }

    public void PrintMultiClassClassificationMetrics(string name, MulticlassClassificationMetrics metrics)
    {
      Console.WriteLine($"************************************************************");
      Console.WriteLine($"*    Metrics for {name} multi-class classification model   ");
      Console.WriteLine($"*-----------------------------------------------------------");
      Console.WriteLine($"    AccuracyMacro = {metrics.MacroAccuracy:0.####}, a value between 0 and 1, the closer to 1, the better");
      Console.WriteLine($"    AccuracyMicro = {metrics.MicroAccuracy:0.####}, a value between 0 and 1, the closer to 1, the better");
      Console.WriteLine($"    LogLoss = {metrics.LogLoss:0.####}, the closer to 0, the better");

      int i = 0;
      foreach (var classLogLoss in metrics.PerClassLogLoss)
      {
        i++;
        Console.WriteLine($"    LogLoss for class {i} = {classLogLoss:0.####}, the closer to 0, the better");
      }
      Console.WriteLine($"************************************************************");
    }

  }
}
