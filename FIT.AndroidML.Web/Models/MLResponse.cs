using FIT.Android.Models.ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIT.AndroidML.Web.Models
{
  public class MLResponse
  {
    public bool Status { get; set; } = false;
    public string Message { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;

    public bool IsWinston { get; set; } = false;
    public string Header { get; set; } = string.Empty;
    public string SubHeader { get; set; } = string.Empty;

    public string WinstonVj { get; set; } = string.Empty;
    public string UpaljacVj { get; set; } = string.Empty;
    public string OstaloVj { get; set; } = string.Empty;
    public ImagePrediction Response { get;set;} = null;

  }
}
