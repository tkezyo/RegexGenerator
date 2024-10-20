using Serilog.Events;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ty;
using Microsoft.Extensions.Hosting;

namespace RegexGenerator
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var configuration = new LoggerConfiguration()
#if DEBUG
           .MinimumLevel.Debug()
#else
            .MinimumLevel.Information()
#endif
           .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
           //输出到文件
           .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day)
           .Enrich.FromLogContext();

            var host = await ApplicationHostBuilder.CreateApplicationHost<RegexGeneratorModule>(args,  skipVerification: true) ?? throw new Exception();

            Thread thread = new(async () =>
            {
                await host.RunAsync();
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();

        }
    }
}
