using FIT.AndroidML.MLCore;
using System;

namespace FIT.AndroidML.Trainer
{
  class Program
  {
    static void Main(string[] args)
    {
      MLFileManager.SetPaths(typeof(Program).Assembly);
      MLTrainer trainer = new MLTrainer();

      trainer.LoadIntoMemory();
      trainer.TransformDataByLabels();
      trainer.GenerateTrainTestDataAndInvokePipeline();

      trainer.StartTraining();
      trainer.Evaluation();

      trainer.Save();

    }
  }
}
