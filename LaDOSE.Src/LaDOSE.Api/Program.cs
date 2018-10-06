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
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();
            //var certificateSettings = config.GetSection("CertificateSettings");
            //string certificateFileName = certificateSettings.GetValue<string>("filename");
            //string certificatePassword = certificateSettings.GetValue<string>("password");

            //X509Certificate2 certificate = new X509Certificate2(certificateFileName, certificatePassword);
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseContentRoot(Directory.GetCurrentDirectory())
            .UseConfiguration(config)
                .UseKestrel(options => options.Listen(IPAddress.Loopback,int.Parse(config["Port"])))
                    .UseStartup<Startup>();
        }
    }
}
