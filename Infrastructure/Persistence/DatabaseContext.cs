﻿using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Domain.BaseModels;
using Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class DatabaseContext : IdentityDbContext<BaseUser>, IDatabaseContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> option) : base(option)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<BaseUser> BaseUsers { get; set; }
        public DbSet<Event> Events { get; set; }
    }
}