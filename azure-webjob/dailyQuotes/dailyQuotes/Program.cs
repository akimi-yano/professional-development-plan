using System;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace dailyQuotes
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new HostBuilder();
            builder.ConfigureWebJobs(bldr =>
            {
                bldr.AddSendGrid();
                bldr.AddTimers();
                bldr.AddAzureStorageCoreServices();
                bldr.AddAzureStorage();
            });
            builder.ConfigureLogging(logcfg =>
            {
                logcfg.AddConsole();
            });
            var host = builder.Build();
            using (host)
            {
                host.Run();
            }

        }
    }
}
