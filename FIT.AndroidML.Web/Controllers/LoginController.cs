using FIT.AndroidML.Web.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIT.AndroidML.Web.Controllers
{
  [EnableCors("default")]
  [Route("api/login")]
  public class LoginController : ControllerBase
  {

    [HttpGet("{username}/{password}")]
    public LoginResult TryLogin(string username, string password)
    {
      if (username.Equals("admin") && password.Equals("root"))
        return new LoginResult() { Status = true };
      else
        return new LoginResult() { Status = false };
    }

  }
}