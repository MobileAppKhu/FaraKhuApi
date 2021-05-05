using System.IO;
using System.Linq;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebApi;
using DatabaseInitializer = UnitTest.Persistence.DatabaseInitializer;

namespace UnitTest.Utilities
{
    public class AppFactory
    {
        protected IHost Host;

        protected AppFactory()
        {
            ConfigureWebHost();
            
            new DatabaseInitializer(Host.Services).Initialize().GetAwaiter().GetResult();
        }

        private void ConfigureWebHost()
        {
            var hostBuilder = new HostBuilder()
                .ConfigureWebHost(webHost =>
                {
                    // Add TestServer
                    webHost.UseTestServer();
                    webHost.UseStartup<Startup>();
                    webHost.UseConfiguration(new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", true)
                        .Build());

                    // configure the services after the startup has been called.
                    webHost.ConfigureTestServices(services =>
                    {
                        // Remove the app's ApplicationDbContext registration.
                        var descriptor = services.SingleOrDefault(d
                            => d.ServiceType == typeof(DbContextOptions<DatabaseContext>));

                        if (descriptor != null)
                            services.Remove(descriptor);
                        
                        descriptor = services.SingleOrDefault(d
                            => d.ServiceType == typeof(DbContextOptions<DatabaseContext>));

                        if (descriptor != null)
                            services.Remove(descriptor);

                        // Add In memory DataBase for testing
                        services.AddDbContext<DatabaseContext>(options =>
                        {
                            options.UseInMemoryDatabase("InMemoryDbForTesting");
                        });
                    });
                });

            // Build and start the IHost
            Host = hostBuilder.Start();
        }
    }
}
