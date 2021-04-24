using System;
using Application.Common.Interfaces;
using Application.Common.Interfaces.IServices;
using Infrastructure.Persistence;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DatabaseContext>(option =>
            {
                var stringConnection = Environment.GetEnvironmentVariable("DefaultConnection");
                option.UseNpgsql(stringConnection ?? configuration.GetConnectionString("DefaultConnection"));

            });
            
        }
    }
}