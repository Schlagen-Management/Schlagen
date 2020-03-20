using Blazored.Toast;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Schlagen.Areas.Identity;
using Schlagen.Data;
using Schlagen.Services;
using System;
using System.Collections.Generic;
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
            //services.AddApplicationInsightsTelemetry();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<IdentityUser>(
                options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddScoped<HttpClient>(s =>
            {
                var navigator = s.GetRequiredService<NavigationManager>();
                return new HttpClient { BaseAddress = new Uri(navigator.BaseUri) };
            });

            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddBlazoredToast();

            services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();
            //services.AddScoped<IEmploymentServices, EmploymentServices>();
            services.AddScoped<IInformationRequestServices, InformationRequestServices>();
            services.AddScoped<AzureServiceTokenProvider>();
            services.AddSingleton<IEmailService, EmailService>();
            services.AddScoped<ToastService>();
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

            //// Perform database migration if required
            //var dbContext
            //    = serviceProvider.GetRequiredService<ApplicationDbContext>();
            //dbContext.Database.Migrate();

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
