using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyToDo.Api
{
  public class Program
  {
    public static void Main(string[] args)
    {
      var config = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("hosting.json", true)
        .Build();
      CreateHostBuilder(args, config).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args, IConfiguration config) =>
        Host.CreateDefaultBuilder(args)
        
        .ConfigureWebHostDefaults(webBuilder =>
        {
          Console.WriteLine(config);
          webBuilder.UseConfiguration(config);
          webBuilder.UseStartup<Startup>();
        });
  }
}
