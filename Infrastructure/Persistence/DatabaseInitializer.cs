using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Domain.BaseModels;
using Domain.Enum;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistence
{
    public class DatabaseInitializer
    {
        private RoleManager<IdentityRole> RoleManager { get; }
        private DatabaseContext DatabaseContext { get; }
        private IConfiguration Configuration { get; }
        private UserManager<BaseUser> UserManager { get; }

        public DatabaseInitializer(IServiceProvider scopeServiceProvider)
        {
            RoleManager = scopeServiceProvider.GetService<RoleManager<IdentityRole>>();
            DatabaseContext = scopeServiceProvider.GetService<DatabaseContext>();
            Configuration = scopeServiceProvider.GetService<IConfiguration>();
            UserManager = scopeServiceProvider.GetService<UserManager<BaseUser>>();
        }

        public async Task Initialize()
        {
            await DatabaseContext.Database.MigrateAsync();
            //await DatabaseContext.Database.EnsureDeletedAsync();
            //await DatabaseContext.Database.EnsureCreatedAsync();
            if(DatabaseContext.UserRoles.Any())
                return;
            await RoleInitializer();
            await AvatarInitializer();
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
            var avatar = await DatabaseContext.Files.FirstOrDefaultAsync(a => a.Id == "smiley.png");
            var officer = new BaseUser
            {
                FirstName = "PublicRelation",
                LastName = "Officer",
                Email = "PublicRelation@FaraKhu.app",
                UserType = UserType.PROfficer,
                EmailConfirmed = true,
                Avatar = avatar,
                AvatarId = "smiley.png"
            };
            await UserManager.CreateAsync(officer, "PROfficerPassword");
            await UserManager.AddToRoleAsync(officer, UserType.PROfficer.ToString().Normalize());

            var owner = new BaseUser
            {
                FirstName = "Owner",
                LastName = "User",
                Email = "Owner@FaraKhu.app",
                UserType = UserType.Owner,
                EmailConfirmed = true,
                Avatar = avatar,
                AvatarId = "smiley.png"

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
                EmailConfirmed = true,
                Avatar = avatar,
                AvatarId = "smiley.png"
            };

            await UserManager.CreateAsync(instructor, "InstructorPassword");
            await UserManager.AddToRoleAsync(instructor, UserType.Instructor.ToString().Normalize());
            
            var student = new Student()
            {
                FirstName = "Student",
                LastName = "User",
                Email = "Student@FaraKhu.app",
                UserType = UserType.Student,
                StudentId = "12345",
                EmailConfirmed = true,
                Avatar = avatar,
                AvatarId = "smiley.png"

            };

            await UserManager.CreateAsync(student, "StudentPassword");
            await UserManager.AddToRoleAsync(student, UserType.Student.ToString().Normalize());

        }

        private async Task AvatarInitializer()
        {
            var smiley = await UploadFile("/Persistence/Files/smiley.png", FileType.Image);
            var sad = await UploadFile("/Persistence/Files/sad.png", FileType.Image); 
            var blink = await UploadFile("/Persistence/Files/blink.png", FileType.Image); 
            var poker = await UploadFile("/Persistence/Files/poker.png", FileType.Image);

            await DatabaseContext.Files.AddAsync(smiley);
            await DatabaseContext.Files.AddAsync(sad);
            await DatabaseContext.Files.AddAsync(blink);
            await DatabaseContext.Files.AddAsync(poker);
            await DatabaseContext.SaveChangesAsync();
        }
        
        private async Task<FileEntity> UploadFile(string path, FileType fileType)
        {
            var directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var fileStream = System.IO.File.OpenRead(directory + path);
            var avatar = new FileEntity
            {
                Id = Path.GetFileName(fileStream.Name),
                Type = fileType,
                Size = fileStream.Length,
                ContentType = "image/jpeg",
                Name = Path.GetFileName(fileStream.Name)
            };
            await DatabaseContext.Files.AddAsync(avatar);
            var stream = System.IO.File.Create(Configuration["StorePath"] + avatar.Id);
            await fileStream.CopyToAsync(stream);
            return avatar;
        }
    }
}