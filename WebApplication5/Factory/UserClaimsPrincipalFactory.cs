using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication5.Models;

namespace WebApplication5.Factory
{
    public class UserClaimsPrincipalFactory: UserClaimsPrincipalFactory<User, IdentityRole>
    {
        public UserClaimsPrincipalFactory(UserManager<User> userManager,
           RoleManager<IdentityRole> roleManager,
           IOptions<IdentityOptions> optionsAccessor)
           : base(userManager, roleManager, optionsAccessor)
        {

        }
    }
}
