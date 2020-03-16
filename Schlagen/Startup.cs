using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Schlagen.Areas.Identity;
using Schlagen.Data;
using Schlagen.Services;
using System.Net.Http;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Azure.KeyVault.Models;

namespace Schlagen
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
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
            services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();
            //services.AddScoped<IEmploymentServices, EmploymentServices>();
            services.AddScoped<IInformationRequestServices, InformationServices>();
            services.AddScoped<AzureServiceTokenProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider )
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

            // Perform database migration if required
            var dbContext 
                = serviceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Database.Migrate();

            //Create the required user roles
            CreateRolesAsync(serviceProvider, env).Wait();
        }

        private async Task CreateRolesAsync(IServiceProvider serviceProvider, IWebHostEnvironment env)
        {
            string adminEmail = string.Empty;
            string adminPassword = string.Empty;

            if (env.IsDevelopment())
            {
                adminEmail = Configuration.GetSection("AppSettings")["AdminEmail"];
                adminPassword = Configuration.GetSection("AppSettings")["AdminPwd"];
            }
            else
            {
                try
                {
                    var azureServiceTokenProvider
                        = serviceProvider.GetRequiredService<AzureServiceTokenProvider>();

                    /* The next four lines of code show you how to use AppAuthentication library to fetch secrets from your key vault */
                    KeyVaultClient keyVaultClient
                        = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(
                        azureServiceTokenProvider.KeyVaultTokenCallback));
                    var adminEmailSecret
                        = await keyVaultClient.GetSecretAsync(
                            "https://aickeyvault.vault.azure.net/secrets/AdminEmail/2fd915c5210a4034a1024b06e8161e79")
                            .ConfigureAwait(false);
                    adminEmail = adminEmailSecret.Value;
                    var adminPwdSecret
                        = await keyVaultClient.GetSecretAsync(
                            "https://aickeyvault.vault.azure.net/secrets/AdminPwd/294d4ad990b84ce9b105e56ecd27438c")
                            .ConfigureAwait(false);
                    adminPassword = adminPwdSecret.Value;
                }
                /* If you have throttling errors see this tutorial https://docs.microsoft.com/azure/key-vault/tutorial-net-create-vault-azure-web-app */
                /// <exception cref="KeyVaultErrorException">
                /// Thrown when the operation returned an invalid status code
                /// </exception>
                catch (KeyVaultErrorException keyVaultException)
                {
                    // TODO: Need to log an error here
                }
            }

            var userManager 
                = serviceProvider
                .GetRequiredService<UserManager<IdentityUser>>();
            var roleManager
                = serviceProvider
                .GetRequiredService<RoleManager<IdentityRole>>();

            List<string> roleNames = new List<string> { "Admin", "ContentCreator" };

            IdentityResult result;

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
                        result
                            = await userManager.AddToRoleAsync(sysAdmin, "Admin");
                    }
                }
            }
        }
    }
}
