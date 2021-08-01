using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer.Identity;
using Microsoft.AspNetCore.Identity;

namespace IdentityServer
{
    public class Seed
    {
        public static async Task SeedUsersAsync(UserManager<ApplicationUser> userManager)
        {
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
                        
                var devClaims = new List<Claim>
                {
                    new("Developer", "true")
                };
                        
                //creation
                await userManager.CreateAsync(developerUser, "password");
                await userManager.AddClaimsAsync(developerUser, devClaims);
                
                var normalUser = new ApplicationUser
                {
                    UserName = "normal",
                    Email = "normal@gmail.com",
                    FirstName = "John",
                    LastName = "Smith",
                    CreateDate = DateTime.Now
                };
                await userManager.CreateAsync(normalUser, "password");
                
                var godUser = new ApplicationUser
                {
                    UserName = "god",
                    Email = "god@hasnoemail.com",
                    FirstName = "God",
                    LastName = "None",
                    CreateDate = DateTime.Now
                };
                var godClaims = new List<Claim>
                {
                    new("Developer", "true"),
                    new("Scientist", "true")
                };
                await userManager.CreateAsync(godUser, "password");
                await userManager.AddClaimsAsync(godUser, godClaims);
                
            }
        }
    }
}