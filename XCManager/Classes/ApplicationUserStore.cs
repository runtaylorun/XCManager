using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using XCManager.Models;
using System.Threading.Tasks;

namespace XCManager.Classes
{
    public class ApplicationUserStore : UserStore<ApplicationUser>
    {
        public ApplicationUserStore(DbContext context)
            : base(context)
        {

        }

        public override async Task<ApplicationUser> FindByIdAsync(string userId)
        {
           
            return await Users.Include(r => r.Roles).Include(r => r.Claims).Include(r => r.Logins).FirstOrDefaultAsync(r => r.Id == userId);
        }

        
    }
}