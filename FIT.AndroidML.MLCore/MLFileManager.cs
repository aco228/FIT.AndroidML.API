using System;
using System.Collections.Generic;
using System.Text;

namespace FIT.AndroidML.MLCore
{
  public static class MLFileManager
  {
    public static readonly string AssetsRoot = @"D:\github\FIT.AndroidML.API\FIT.AndroidML.API\assets\";
    public static string InputFolder { get => AssetsRoot + @"\inputs"; }
    public static string OutputFolder { get => AssetsRoot + @"\output"; }
    public static string UploadFolder { get => AssetsRoot + @"\uploads"; }
    public static string ModelLocation { get => AssetsRoot + @"\model.zip"; }

  }
}
