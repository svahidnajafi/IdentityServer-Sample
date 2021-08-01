using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WebClientMVC
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = "Cookie";
                    options.DefaultChallengeScheme = "oidc";
                })
                .AddCookie("Cookie")
                .AddOpenIdConnect("oidc", config =>
                {
                    // setup
                    config.Authority = Configuration["AuthenticationConfig:Authority"];
                    config.ResponseType = "code";
                    config.ClientId = Configuration["AuthenticationConfig:ClientId"];
                    config.ClientSecret = Configuration["AuthenticationConfig:ClientSecret"];
                    config.SignedOutRedirectUri = "/home/index";
                    config.SaveTokens = true;
                    
                    // mapping user claims
                    config.GetClaimsFromUserInfoEndpoint = true;
                    config.ClaimActions.MapAll();
                    config.Scope.Add("userClaims");
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(PolicyStore.TopSecret, config =>
                {
                    config.RequireAuthenticatedUser();
                    config.RequireClaim("Developer", "true");
                });
                
                options.AddPolicy(PolicyStore.God, config =>
                {
                    config.RequireAuthenticatedUser();
                    config.RequireClaim("Developer", "true");
                    config.RequireClaim("Scientist", "true");
                });
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

            app.UseEndpoints(endpoints => { endpoints.MapDefaultControllerRoute(); });
        }
    }
}