using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;
using XCManager.Models;

namespace XCManager.Controllers
{
    
    public class HomeController : Controller
    {
        private ApplicationDbContext _context;
        private UserManager<ApplicationUser> UserManager { get; set; }

        public HomeController()
        {
            _context = new ApplicationDbContext();
            UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
        }
        [AllowAnonymous]
        public ActionResult Index()
        {
           
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }
        [Authorize]
        public async Task<ActionResult> TeamHome()
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user.Team == null)
            {
                return RedirectToAction("NewTeamForm", "Account");
            }
            return View();
        }
    }
}