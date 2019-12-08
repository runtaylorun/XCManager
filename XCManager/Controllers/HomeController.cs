using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;
using XCManager.Models;
using XCManager.Models.ViewModels;

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
            TeamHomeViewModel teamHomeViewModel;
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            var teamRaceSchedule = _context.Races.Where(r => r.Team.Id == user.Team.Id).ToList();
            var nextRace = CalculateNextRace(teamRaceSchedule);

            if (user.Team == null)
            {
                return RedirectToAction("NewTeamForm", "Account");
            }
            else
            {
                teamHomeViewModel = new TeamHomeViewModel()
                {
                    Team = _context.Teams.SingleOrDefault(t => t.Id == user.Team.Id),
                    VarsityRunners = _context.Runners.Where(r => r.Team.Id == user.Team.Id).Take(7).ToList(),
                    NextRace = nextRace
                };
            }
            return View(teamHomeViewModel);
        }

        private Race CalculateNextRace(List<Race> teamRaceSchedule)
        {
            var currentDate = DateTime.Now;
            var smallestDifference = teamRaceSchedule[0].Date.Subtract(currentDate);
            var nextRace = teamRaceSchedule[0];


            for(int i = 0; i < teamRaceSchedule.Count; i++)
            {
                var currentDifference = teamRaceSchedule[i].Date.Subtract(currentDate);

                if(currentDifference.Ticks < smallestDifference.Ticks)
                {
                    smallestDifference = currentDifference;
                    nextRace = teamRaceSchedule[i];
                }
            }

            return nextRace;
        }
    }
}