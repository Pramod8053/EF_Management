using HospitalManagement.Repositories;
using HospitalManagement.Repositories.EnrollRepository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalManagement
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddScoped<IlogiRepository, logiRepository>();
            services.AddScoped<IEnrollRepo,EnrollRepo>();
            // Add session services
            services.AddSession(options =>
            {
                // Set session timeout value (optional)
                options.IdleTimeout = TimeSpan.FromMinutes(5); // Adjust as needed
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true; // Important for GDPR compliance
            });
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
              .AddCookie(options =>
              {
                  options.Cookie.Name = "HospitalManagementAuthCookie";
                  options.ExpireTimeSpan = TimeSpan.FromMinutes(1);
                  options.LoginPath = "/login/index"; // Redirect to login page if unauthorized
                  options.AccessDeniedPath = "/login/AccessDenied"; // Redirect if access denied
              });
            services.AddHttpContextAccessor();
            // Default authorization services
            services.AddAuthorization();
            var policy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        //.RequireRole("Administrator", "Manager")
                        .Build();
            services.AddMvc(options =>
            {
                options.Filters.Add(new AuthorizeFilter(policy));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,ILoggerFactory loggerFactory)
        {
            loggerFactory.AddFile("wwwroot/Logfile/log.txt");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseAuthentication(); // Enable authentication middleware
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=login}/{action=Index}/{id?}");
                
            });
        }
    }
}
