using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using FIT.AndroidML.MLCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FIT.AndroidML.Web
{
  public class Program
  {
    public static string IP;
    public static MLPredictionEngine PredictionEngine;

    public static void Main(string[] args)
    {
      MLFileManager.SetPaths(typeof(Program).Assembly);
      PredictionEngine = new MLPredictionEngine();

      var configuration = new ConfigurationBuilder()
        .AddCommandLine(args)
        .Build();

      using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
      {
        socket.Connect("8.8.8.8", 65530);
        IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
        IP = endPoint.Address.ToString();
      }

      Console.WriteLine("Server at:");
      Console.WriteLine(IP);
      Console.WriteLine("");
      Console.WriteLine("");

      var host = new WebHostBuilder()
          .UseKestrel()
          .UseUrls("https://*:5001")
          .UseContentRoot(Directory.GetCurrentDirectory())
          .UseIISIntegration()
          .UseStartup<Startup>()
          .UseConfiguration(configuration)
          .Build();

      host.Run();
      //CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
              webBuilder.UseStartup<Startup>();
            });
  }
}
