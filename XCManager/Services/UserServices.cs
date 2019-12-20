using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using XCManager.Models;

namespace XCManager.Services
{
    public interface IUserServices
    {
        Task<ApplicationUser> GetUser();
    }

    public class UserServices : IUserServices
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserServices(ApplicationDbContext context)
        {
            _context = context;
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
        }

        public async Task<ApplicationUser> GetUser()
        {
            return await _userManager.FindByIdAsync(System.Web.HttpContext.Current.User.Identity.GetUserId());
        }
    }
}