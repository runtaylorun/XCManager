using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using XCManager.Models;
using XCManager.Models.ViewModels;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace XCManager.Controllers
{
    public class RosterController : Controller
    {
        private ApplicationDbContext _context;
        private UserManager<ApplicationUser> UserManager { get; set; }

        public RosterController()
        {
            _context = new ApplicationDbContext();
            UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
        }

        public async Task<ActionResult> Index()
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            RosterViewModel runners = new RosterViewModel
            {
                Runners = _context.Runners.Where(r => r.Team.Id == user.Team.Id).ToList()
            };
            return View(runners);
        }

        public ActionResult NewRunner()
        {
            return View("RunnerForm");
        }

        public ActionResult UpdateRunner(int id)
        {
            var runner = _context.Runners.SingleOrDefault(r => r.Id == id);
            return View("RunnerForm", runner);
        }

        public ActionResult DeleteRunner(int id)
        {
            var runnerToDelete = _context.Runners.SingleOrDefault(r => r.Id == id);

            _context.Runners.Remove(runnerToDelete);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Save(Runner runner)
        {
            if (!ModelState.IsValid)
            {
                foreach (ModelState modelState in ViewData.ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                    {
                        continue;
                    }
                }
                return RedirectToAction("NewRunner", runner);
            }
            else
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                runner.Team = user.Team;
                if (runner.Id == null || runner.Id == 0)
                    _context.Runners.Add(runner);
                else
                {
                    var runnerToUpdate = _context.Runners.SingleOrDefault(r => r.Id == runner.Id);

                    runnerToUpdate.Name = runner.Name;
                    runnerToUpdate.Grade = runner.Grade;
                    runnerToUpdate.Email = runner.Email;
                }

                _context.SaveChanges();

                return RedirectToAction("Index");
            }
            
        }

        public ActionResult RunnerDetails(int id)
        {
            RunnerViewModel runner = new RunnerViewModel
            {
                Runner = _context.Runners.SingleOrDefault(r => r.Id == id),
                RecentResults = _context.IndividualResults.Include(r => r.Race).OrderByDescending(r => r.Race.Date).Where(r => r.runner.Id == id).Take(3).ToList(),
                PersonalBests = new Dictionary<string, TimeSpan>()

            };
            var Distances = _context.Races.Select(r => r.Distance).Distinct().ToList();
            foreach(string distance in Distances)
            {
                var bestTime = (from a in _context.IndividualResults
                                join c in _context.Races on a.Race.Id equals c.Id
                                join d in _context.Runners on a.runner.Id equals d.Id
                                where c.Distance == distance && a.runner.Id == id
                                orderby a.finishingTime ascending
                                select a.finishingTime).FirstOrDefault();

                runner.PersonalBests.Add(distance, bestTime);               
            }
            
            
            return View(runner);
        }
    }
}