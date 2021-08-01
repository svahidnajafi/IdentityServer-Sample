using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer.Configurations;
using IdentityServer.Data;
using IdentityServer.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IdentityServer
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseInMemoryDatabase("IdentityServerSample");
            });

            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
                {
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequiredUniqueChars = 0;
                    options.Password.RequiredLength = 4;
                    options.Password.RequireDigit = false;
                    options.Password.RequireUppercase = false;
                })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services.AddIdentityServer()
                .AddAspNetIdentity<ApplicationUser>()
                .AddInMemoryApiScopes(IdentityServerConfig.ApiScopes)
                .AddInMemoryApiResources(IdentityServerConfig.ApiResources)
                .AddInMemoryIdentityResources(IdentityServerConfig.IdentityResources)
                .AddInMemoryClients(IdentityServerConfig.Clients)
                .AddDeveloperSigningCredential();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseIdentityServer();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapDefaultControllerRoute(); });
        }
    }
}