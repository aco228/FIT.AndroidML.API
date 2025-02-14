﻿using System;
using System.Collections.Generic;
using System.Text;

namespace FIT.Android.Models.ML
{
  public class InMemoryImageData
  {
    public InMemoryImageData(byte[] image, string label, string imageFileName)
    {
      Image = image;
      Label = label;
      ImageFileName = imageFileName;
    }

    public readonly byte[] Image;

    public readonly string Label;

    public readonly string ImageFileName;
  }
}
