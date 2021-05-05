using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Domain.BaseModels;
using Domain.Models;
using Infrastructure.Identity;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace UnitTest.Persistence
{
    public class DatabaseInitializer
    {
        private object Lock { get; } = new();
        private DatabaseContext Context { get; }
        private IConfiguration Configuration { get; }
        private UserManager<BaseUser> UserManager { get; }

        private RoleManager<IdentityRole> RoleManager { get; }
        // private ILoggerService<DatabaseInitializer> Logger { get; }

        public DatabaseInitializer(IServiceProvider scopeServiceProvider)
        {
            Context = scopeServiceProvider.GetService<DatabaseContext>();
            Configuration = scopeServiceProvider.GetService<IConfiguration>();
            UserManager = scopeServiceProvider.GetService<UserManager<BaseUser>>();
            RoleManager = scopeServiceProvider.GetService<RoleManager<IdentityRole>>();
            // Logger = scopeServiceProvider.GetService<ILoggerService<DatabaseInitializer>>();
        }

        public async Task Initialize()
        {
            await Context.Database.EnsureDeletedAsync();
            await Context.Database.EnsureCreatedAsync();
            await RoleInitializer();
            await UserInitializer();
        }

        private async Task RoleInitializer()
        {
            await RoleManager.CreateAsync(new IdentityRole {Name = "Owner"});
            await RoleManager.CreateAsync(new IdentityRole {Name = "Instructor"});
            await RoleManager.CreateAsync(new IdentityRole {Name = "Student"});
            await RoleManager.CreateAsync(new IdentityRole {Name = "PROfficer"});
        }

        private async Task UserInitializer()
        {
            var owner = new BaseUser()
            {
                Id = "OwnerUser",
                UserName = "",
                LastName = "Owner",
                FirstName = "Owner",
                Email = "TestOwner@Test.com"
            };
            await UserManager.CreateAsync(owner, "OwnerPassword");
            await UserManager.AddToRoleAsync(owner, "Owner");
            var ownerToken1 = await UserManager.GenerateEmailConfirmationTokenAsync(owner);
            await UserManager.ConfirmEmailAsync(owner, ownerToken1);

            var student = new Student
            {
                Id = "StudentUser",
                UserName = "",
                LastName = "Test",
                FirstName = "User1",
                Email = "TestUser@Test.com",
            };
            await UserManager.CreateAsync(student, "StudentPassword");
            await UserManager.AddToRoleAsync(student, "Student");
            var userToken1 = await UserManager.GenerateEmailConfirmationTokenAsync(student);
            await UserManager.ConfirmEmailAsync(student, userToken1);

            var instructor = new Instructor
            {
                Id = "InstructorUser",
                UserName = "",
                LastName = "Test",
                FirstName = "User2",
                Email = "UnconfirmedEmail@Test.com"
            };
            await UserManager.CreateAsync(instructor, "InstructorPassword");
            await UserManager.AddToRoleAsync(instructor, "Instructor");
        }

        
    }
}