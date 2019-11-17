using FIT.AndroidML.MLCore;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIT.AndroidML.Web.Controllers
{
  [Route("/")]
  public class MainController : ControllerBase
  {

    [Produces("text/html")]
    public IActionResult Index()
      => this.Content(
        System.IO.File.ReadAllText(Path.Combine(MLFileManager.AssetsRoot, "../_testHtml/index.html")).Replace("[IP]", Program.IP), 
        "text/html", 
        Encoding.UTF8);

    [HttpGet("test")]
    public IActionResult Test() => this.Content("OK");

  }
}
