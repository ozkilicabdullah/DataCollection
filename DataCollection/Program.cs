using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace DataCollection
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }
            
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel(options =>
                 {
                    options.Limits.MaxRequestBodySize = 2097152; //2MB
                 })
                .UseIISIntegration()
                .UseStartup<Startup>();
    }
}
