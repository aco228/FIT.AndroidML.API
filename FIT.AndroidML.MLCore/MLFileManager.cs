using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace FIT.AndroidML.MLCore
{
  public static class MLFileManager
  {
    public static string RootFolder = "";
    public static string AssetsRoot { get => RootFolder + @"assets\"; }
    public static string InputFolder { get => AssetsRoot + @"\inputs"; }
    public static string OutputFolder { get => AssetsRoot + @"\output"; }
    public static string UploadFolder { get => AssetsRoot + @"\uploads"; }
    public static string ModelLocation { get => AssetsRoot + @"\model.zip"; }

    public static void SetPaths(Assembly assembly)
    {
      var assemblyFolderPath = new FileInfo(assembly.Location).Directory.FullName;
      string[] parts = assemblyFolderPath.Split('\\');

      string result = "";
      bool done = false;
      foreach(string part in parts)
      {
        result += part + '\\';
        if (part.Equals("FIT.AndroidML.API"))
        {
          done = true;
          break;
        }
      }

      if (!done)
        throw new Exception("Greska root folder");

      RootFolder = result;
    }

  }
}
