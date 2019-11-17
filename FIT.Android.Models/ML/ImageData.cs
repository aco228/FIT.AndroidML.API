using System;
using System.Collections.Generic;
using System.Text;

namespace FIT.AndroidML.Models.ML
{
  public class ImageData
  {
    public ImageData(string imagePath, string label)
    {
      ImagePath = imagePath;
      Label = label;
    }

    public readonly string ImagePath;

    public readonly string Label;
  }
}
