using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application;
using Application.Common.Interfaces;
using Domain.BaseModels;
using Infrastructure;
using Infrastructure.Identity;
using Infrastructure.MiddleWare;
using Infrastructure.Persistence;
using Infrastructure.Policy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WebApi
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        private IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddApplication(Configuration);
            
            services.AddInfrastructureServices(Configuration);

            services.AddScoped<IDatabaseContext, DatabaseContext>();

            services.AddControllers();

            services.AddIdentity<BaseUser, IdentityRole>()
                .AddEntityFrameworkStores<DatabaseContext>()
                .AddUserManager<CustomUserManager>()
                .AddDefaultTokenProviders();

            services.AddScoped<IAuthorizationHandler, RoleAuthorizationHandler>();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 8;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "token";
                options.Cookie.HttpOnly = false;
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.ExpireTimeSpan = TimeSpan.FromDays(365);
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Task.CompletedTask;
                };
                options.Events.OnRedirectToAccessDenied = context =>
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Task.CompletedTask;
                };
            });
            
            services.AddAuthorization(option =>
            {
                option.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .AddRequirements(new AuthorizationRequirements(new List<string> {"Student", "Instructor"}))
                    .Build();
                option.AddPolicy("StudentPolicy", policy =>
                    policy.AddRequirements(new AuthorizationRequirements(new List<string> {"Student"})));
                option.AddPolicy("InstructorPolicy", policy =>
                    policy.AddRequirements(new AuthorizationRequirements(new List<string> {"Instructor"})));
                
            });
            
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseMiddleware<CustomExceptionMiddleWare>();

            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action}/{id?}");
            });
        }
    }
}