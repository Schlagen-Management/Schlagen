using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Schlagen.Areas.Identity;
using Schlagen.Data;
using Schlagen.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Schlagen
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            if (Environment.GetEnvironmentVariable("DATABASE_ENVIRONMENT") == "Development")
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(
                    Configuration.GetConnectionString("ProdConnection")));
            else
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(
                    Configuration.GetConnectionString("DevConnection")));

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
                //.AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();

            services.AddScoped<IInformationRequestServices, InformationRequestServices>();
            services.AddSingleton<IEmailService, EmailService>();

            //services.BuildServiceProvider().GetService<ApplicationDbContext>().Database.Migrate();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });

            //// Create the required user roles
            //CreateRolesAsync(serviceProvider, env).Wait();
        }

        private async Task CreateRolesAsync(IServiceProvider serviceProvider, IWebHostEnvironment env)
        {
            var adminEmail = Configuration.GetSection("AppSettings")["AdminEmail"];
            var adminPassword = Configuration.GetSection("AppSettings")["AdminPwd"];

            var userManager 
                = serviceProvider
                .GetRequiredService<UserManager<IdentityUser>>();
            var roleManager
                = serviceProvider
                .GetRequiredService<RoleManager<IdentityRole>>();

            List<string> roleNames = new List<string> { "Admin", "ContentCreator" };

            IdentityResult result;

            // Create the desired roles
            foreach (var roleName in roleNames)
            {
                if (await roleManager.RoleExistsAsync(roleName) == false)
                    result = await roleManager.CreateAsync(new IdentityRole(roleName));
            }

            // Create admin site user
            if (string.IsNullOrEmpty(adminEmail) == false
                && string.IsNullOrEmpty(adminPassword) == false)
            {
                var sysAdmin = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                if (await userManager.FindByEmailAsync(adminEmail) == null)
                {
                    var createdAdminUser
                        = await userManager.CreateAsync(sysAdmin, adminPassword);

                    if (createdAdminUser.Succeeded)
                    {
                        // Assign the user to the admin role
                        result
                            = await userManager.AddToRoleAsync(sysAdmin, "Admin");
                    }
                }
            }
        }
    }
}
