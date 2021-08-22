using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

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
                .AddCookie("Cookie", config =>
                {
                    // config.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                    // config.SlidingExpiration = true;
                })
                .AddOpenIdConnect("oidc", config =>
                {
                    // setup
                    config.Authority = Configuration["AuthenticationConfig:Authority"];
                    config.ResponseType = OpenIdConnectResponseType.Code;
                    config.ClientId = Configuration["AuthenticationConfig:ClientId"];
                    config.ClientSecret = Configuration["AuthenticationConfig:ClientSecret"];
                    config.SignedOutRedirectUri = "/home/index";
                    config.SaveTokens = true;
                    config.UseTokenLifetime = true;
                    config.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };

                    // mapping user claims
                    config.GetClaimsFromUserInfoEndpoint = true;
                    config.ClaimActions.MapAll();
                    config.Scope.Add("userClaims");

                    // refresh token
                    config.Scope.Add("offline_access");
                });

            services.AddAuthorization(options =>
            {
                // var defaultAuthorizationPolicyBuilder = new AuthorizationPolicyBuilder
                // ("Cookie");
                // defaultAuthorizationPolicyBuilder =
                //     defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();
                // options.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();
                
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