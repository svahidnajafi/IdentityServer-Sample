using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace IdentityServer
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // var config = new ConfigurationBuilder()
            //     .AddJsonFile("appsettings.json").Build();
            
            try
            {
                var host = CreateHostBuilder(args).Build();

                using (var scope = host.Services.CreateScope())
                {
                    var userManager = scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();

                    //Developer Seeding
                    if (await userManager.FindByNameAsync("dev") == null)
                    {
                        var developerUser = new ApplicationUser
                        {
                            UserName = "dev",
                            Email = "Vahidnajafi.work@gmail.com",
                            PhoneNumber = "+989307198828",
                            FirstName = "Vahid",
                            LastName = "Najafi",
                            CreateDate = DateTime.Now
                        };
                        
                        //creation
                        await userManager.CreateAsync(developerUser, "password");
                        // await userManager.AddClaimsAsync(developerUser, devClaims);
                    }
                } 
                
                await host.RunAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
