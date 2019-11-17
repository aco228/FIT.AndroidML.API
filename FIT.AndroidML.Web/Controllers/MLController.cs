using FIT.AndroidML.MLCore;
using FIT.AndroidML.Web.Code;
using FIT.AndroidML.Web.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FIT.AndroidML.Web.Controllers
{
  [Route("api/ml")]
  public class MLController : ControllerBase
  {

    [EnableCors("default")]
    [HttpPost("")]
    public async Task<MLResponse> UploadImage(IFormFile file)
    {

      Console.WriteLine("--> ML:: Receiving file");

      if(file == null || file.Length == 0)
        return new MLResponse() { Status = false, Message = "Nema fajla!!" };

      if (!CheckIfImageFile(file))
        return new MLResponse() { Status = false, Message = "Samo jpg i png formati!" };

      try
      {
        string fileName = await this.WriteFile(file);
        var response = Program.PredictionEngine.Predict(Path.Combine(MLFileManager.UploadFolder, fileName));

        Console.WriteLine("--> ML:: Has prediction");

        return new MLResponse() { 
          Status = true, Message = "ok", FileName = fileName, Response = response,
          Header = response.IsWinston ? "ЈЕСТЕ" : "НИЈЕ",
          SubHeader = response.IsWinston ? "Ово је винстон дуги тамни" : 
            (response.IsUpaljac ? "Није винстон, али изгледа да је упаљач" : "Нешто непознато"),

          IsWinston = response.IsWinston,
          
          OstaloVj = (response.Score[0] * 100).ToString("0.0") + "%",
          WinstonVj = (response.Score[1] * 100).ToString("0.0") + "%",
          UpaljacVj = (response.Score[2] * 100).ToString("0.0") + "%"
        };
      }
      catch(Exception e)
      {
        return new MLResponse() { Status = false, Message = e.ToString() };
      }
    }

    private bool CheckIfImageFile(IFormFile file)
    {
      byte[] fileBytes;
      using (var ms = new MemoryStream())
      {
        file.CopyTo(ms);
        fileBytes = ms.ToArray();
      }

      var type = ImageUploadHelper.GetImageFormat(fileBytes);
      if (type != ImageUploadHelper.ImageFormat.jpeg && type != ImageUploadHelper.ImageFormat.png)
        return false;

      return true;
    }

    public async Task<string> WriteFile(IFormFile file)
    {
      string fileName;
      var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
      fileName = Guid.NewGuid().ToString() + extension;
      var path = Path.Combine(MLFileManager.UploadFolder, fileName);

      using (var bits = new FileStream(path, FileMode.Create))
        await file.CopyToAsync(bits);

      return fileName;
    }

  }
}
