using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace OtpDemoUi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await RunAppAsync(args);
        }

        public static async Task RunAppAsync(string[] args)
        {
            var cfgBuilder = new ConfigurationBuilder();
            cfgBuilder.SetBasePath(Directory.GetCurrentDirectory());

            var config = cfgBuilder.AddJsonFile("appsettings.json").Build();

            var port = int.Parse(config["Port"]);

            var host = Host
              .CreateDefaultBuilder(args)
              .ConfigureWebHostDefaults(builder =>
              {
                  builder.UseUrls(GetUrls(port, true));
                  builder.UseStartup<Startup>();
              });

            await host.Build().RunAsync();
        }

        private static string[] GetUrls(int port, bool allowDirectPublicAccess)
        {
            List<string> urlList = new List<string>();
            if (allowDirectPublicAccess)
            {
                urlList.Add($"http://0.0.0.0:{port}");
            }
            else
            {
                urlList.Add($"http://127.0.0.1:{port}");
            }

            return urlList.ToArray();
        }
    }
}
