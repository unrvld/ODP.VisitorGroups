using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace DemoSite
{
    public class Program
    {
        public static void Main(string[] args) => CreateHostBuilder(args).Build().Run();

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureCmsDefaults()
                
                .ConfigureWebHostDefaults(webBuilder => 
                 {
                    webBuilder.UseStartup<Startup>();
                 }).ConfigureAppConfiguration((ctx,config) => config.AddUserSecrets("2a3dc6de-40c9-422e-8226-46cc8caa54a9"))
    ;
    }
}
