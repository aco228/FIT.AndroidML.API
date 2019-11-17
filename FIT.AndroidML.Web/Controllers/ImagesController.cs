using FIT.AndroidML.MLCore;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FIT.AndroidML.Web.Controllers
{
  [Route("api/images")]
  public class ImagesController : ControllerBase
  {

    [HttpGet("")]
    public List<string> GetAllImages()
    {
      DirectoryInfo di = new DirectoryInfo(MLFileManager.UploadFolder);
      List<string> result = new List<string>();
      foreach (var f in di.GetFiles())
        result.Add(f.Name);
      return result;
    }

    [HttpGet("{imageName}")]
    public IActionResult GetImage(string imageName)
    {
      string location = Path.Combine(MLFileManager.UploadFolder, imageName);
      FileInfo fi = new FileInfo(location);
      if (!fi.Exists)
        return File(System.IO.File.ReadAllBytes(Path.Combine(MLFileManager.AssetsRoot, "no_image.jpg")), "image/jpeg");
      else
        //return PhysicalFile(location, "image/jpeg");
        return File(System.IO.File.ReadAllBytes(location), "image/jpeg");
    }

  }
}
