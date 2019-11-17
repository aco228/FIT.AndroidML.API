using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace FIT.Android.Models.ML
{
  public class ImagePrediction
  {
    [ColumnName("Score")]
    public float[] Score { get; set; } 

    [ColumnName("PredictedLabel")]
    public string PredictedLabel { get; set; }
    public bool IsWinston { get => this.PredictedLabel.Equals("0.1_winston"); }
    public bool IsUpaljac { get => this.PredictedLabel.Equals("0.2_upaljac"); }

    public bool Validity
    {
      get => this.GetNormilizedScore >= 0.9;
    }

    public string NormalName
    {
      get
      {
        switch (this.PredictedLabel)
        {
          // glupo zakucano
          case "0.1_winston": return "Winston";
          case "0.2_upaljac": return "Upaljac";
          default: return "Nepoznato";
        }
      }
    }

    public double GetNormilizedScore
    {
      get
      {
        switch (this.PredictedLabel)
        {
          // glupo zakucano
          case "0.1_winston": return this.Score[1];
          case "0.2_upaljac": return this.Score[2];
          default: return 0;
        }
      }
    }

    public int Seconds { get; set; }
  }
}
