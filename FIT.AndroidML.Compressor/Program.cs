using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace FIT.AndroidML.Compressor
{
  class Program
  {
    public static string Folder = "0.0_random";

    static void Main(string[] args)
    {
      foreach (var fi in new DirectoryInfo(@"D:\github\FIT.AndroidML.API\FIT.AndroidML.API\assets\inputs\" + Folder).GetFiles())
        ResizeImage(fi.Name);
      //ResizeImage("0d34dc4b-abaf-4ef9-bcb6-e4c88e1a2518.jpg");

      Console.WriteLine("Hello World!");
    }

    public static void ResizeImage(string name)
    {
      Image image = Image.FromFile($@"D:\github\FIT.AndroidML.API\FIT.AndroidML.API\assets\inputs\{Folder}\" + name);
      int width = image.Width / 2;
      int height = image.Height / 2;

      var destRect = new Rectangle(0, 0, width, height);
      var destImage = new Bitmap(width, height);

      destImage.SetResolution(image.VerticalResolution, image.HorizontalResolution);

      using (var graphics = Graphics.FromImage(destImage))
      {
        graphics.CompositingMode = CompositingMode.SourceCopy;
        graphics.CompositingQuality = CompositingQuality.HighSpeed;
        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
        graphics.SmoothingMode = SmoothingMode.HighSpeed;
        graphics.PixelOffsetMode = PixelOffsetMode.HighSpeed;

        using (var wrapMode = new ImageAttributes())
        {
          wrapMode.SetWrapMode(WrapMode.TileFlipXY);
          graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
        }
      }


      var encoder = ImageCodecInfo.GetImageEncoders().First(c => c.FormatID == ImageFormat.Jpeg.Guid);
      var encParams = new EncoderParameters() { Param = new[] { new EncoderParameter(Encoder.Quality, 40L) } };
      destImage.Save($@"C:\Users\aco228_\Desktop\android\inputs\{Folder}\" + name, encoder, encParams);

    }
  }
}
