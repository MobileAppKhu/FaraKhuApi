using System;
using System.Threading.Tasks;
using Domain.BaseModels;
using Domain.Enum;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistence
{
    public class DatabaseInitializer
    {
        private RoleManager<IdentityRole> RoleManager { get; }
        private DatabaseContext DatabaseContext { get; }
        private UserManager<BaseUser> UserManager { get; }

        public DatabaseInitializer(IServiceProvider scopeServiceProvider)
        {
            RoleManager = scopeServiceProvider.GetService<RoleManager<IdentityRole>>();
            DatabaseContext = scopeServiceProvider.GetService<DatabaseContext>();
            UserManager = scopeServiceProvider.GetService<UserManager<BaseUser>>();
        }

        public async Task Initialize()
        {
            //await DatabaseContext.Database.MigrateAsync();
            await DatabaseContext.Database.EnsureDeletedAsync();
            await DatabaseContext.Database.EnsureCreatedAsync();
            await RoleInitializer();
            await UserInitializer();
        }
        private async Task RoleInitializer()
        {
            await RoleManager.CreateAsync(new IdentityRole {Name = "Student".Normalize()});
            await RoleManager.CreateAsync(new IdentityRole {Name = "Instructor".Normalize()});
            await RoleManager.CreateAsync(new IdentityRole {Name = "PROfficer".Normalize()});
            await RoleManager.CreateAsync(new IdentityRole {Name = "Owner".Normalize()});
        }

        private async Task UserInitializer()
        {
            var officer = new BaseUser
            {
                FirstName = "PublicRelation",
                LastName = "Officer",
                Email = "PublicRelation@FaraKhu.app",
                UserType = UserType.PROfficer,
                EmailConfirmed = true
            };
            await UserManager.CreateAsync(officer, "PROfficerPassword");
            await UserManager.AddToRoleAsync(officer, UserType.PROfficer.ToString().Normalize());

            var owner = new BaseUser
            {
                FirstName = "Owner",
                LastName = "User",
                Email = "Owner@FaraKhu.app",
                UserType = UserType.Owner,
                EmailConfirmed = true
            };
            await UserManager.CreateAsync(owner, "OwnerPassword");
            await UserManager.AddToRoleAsync(owner, UserType.Owner.ToString().Normalize());

            var instructor = new Instructor()
            {
                FirstName = "Instructor",
                LastName = "User",
                Email = "Instructor@FaraKhu.app",
                UserType = UserType.Instructor,
                InstructorId = "12345",
                EmailConfirmed = true
            };

            await UserManager.CreateAsync(instructor, "InstructorPassword");
            await UserManager.AddToRoleAsync(instructor, UserType.Instructor.ToString().Normalize());
            
            var student = new Student()
            {
                FirstName = "Instructor",
                LastName = "User",
                Email = "Student@FaraKhu.app",
                UserType = UserType.Instructor,
                StudentId = "12345",
                EmailConfirmed = true
            };

            await UserManager.CreateAsync(student, "StudentPassword");
            await UserManager.AddToRoleAsync(student, UserType.Instructor.ToString().Normalize());

        }
    }
}