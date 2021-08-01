using System;
using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        { }


        public bool? IsActive { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CreateDate { get; set; }
    }
}