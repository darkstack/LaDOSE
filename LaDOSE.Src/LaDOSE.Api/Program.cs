using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace LaDOSE.Api
{
    public class Program
    {
        public static IConfiguration config { get; private set; }

        public static void Main(string[] args)
        {


            config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appSettings.json", optional: true, reloadOnChange: true)
                .Build();
            var certificateSettings = config.GetSection("certificateSettings");
            string certificateFileName = certificateSettings.GetValue<string>("filename");
            string certificatePassword = certificateSettings.GetValue<string>("password");

            X509Certificate2 certificate = new X509Certificate2(certificateFileName, certificatePassword);
            CreateWebHostBuilder(certificate,args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(X509Certificate2 certificate, string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
            .UseConfiguration(config)
                .UseKestrel(options => options.Listen(IPAddress.Loopback,int.Parse(config["Port"]),config=> config.UseHttps(certificate)))
                    .UseStartup<Startup>();
        }
    }
}
