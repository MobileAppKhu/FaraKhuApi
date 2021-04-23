using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistence
{
    public class DatabaseInitializer
    {
        private RoleManager<IdentityRole> RoleManager { get; }

        public DatabaseInitializer(IServiceProvider scopeServiceProvider)
        {
            RoleManager = scopeServiceProvider.GetService<RoleManager<IdentityRole>>();
        }

        public async Task Initialize()
        {
            await RoleInitializer();
        }
        private async Task RoleInitializer()
        {
            await RoleManager.CreateAsync(new IdentityRole {Name = "STUDENT"});
            await RoleManager.CreateAsync(new IdentityRole {Name = "INSTRUCTOR"});
        }
    }
}