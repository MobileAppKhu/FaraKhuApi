﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistence
{
    public class DatabaseInitializer
    {
        private RoleManager<IdentityRole> RoleManager { get; }
        private DatabaseContext DatabaseContext { get; }

        public DatabaseInitializer(IServiceProvider scopeServiceProvider)
        {
            RoleManager = scopeServiceProvider.GetService<RoleManager<IdentityRole>>();
            DatabaseContext = scopeServiceProvider.GetService<DatabaseContext>();
        }

        public async Task Initialize()
        {
            //await DatabaseContext.Database.MigrateAsync();
            await DatabaseContext.Database.EnsureDeletedAsync();
            await DatabaseContext.Database.EnsureCreatedAsync();
            await RoleInitializer();
        }
        private async Task RoleInitializer()
        {
            await RoleManager.CreateAsync(new IdentityRole {Name = "Student".Normalize()});
            await RoleManager.CreateAsync(new IdentityRole {Name = "Instructor".Normalize()});
        }
    }
}