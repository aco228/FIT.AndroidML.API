using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.Text;

namespace FIT.AndroidML.MLCore
{
  public abstract class MLBase
  {
    protected MLContext Context { get; private set; }

    public MLBase()
    {
      this.Context = new MLContext(seed: 1);
      this.Context.Log += Context_Log;
    }

    private void Context_Log(object sender, LoggingEventArgs e)
    {
      if (e.Message.StartsWith("[Source=ImageClassificationTrainer;"))
        Console.WriteLine(e.Message);
    }
  }
}
