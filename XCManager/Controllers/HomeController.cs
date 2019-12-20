using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using XCManager.Models;
using XCManager.Models.ViewModels;
using XCManager.Services;

namespace XCManager.Controllers
{
    
    public class HomeController : Controller
    {
        private ApplicationDbContext _context;
        private readonly IUserServices _userService;

        public HomeController(IUserServices userService)
        {
            _context = new ApplicationDbContext();
            _userService = userService;
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

        public async Task<ActionResult> TeamGoalsForm()
        {
            var user = await _userService.GetUser();
            var goals = _context.TeamGoals.Where(g => g.Team.Id == user.Team.Id).ToList();
            GoalFormViewModel viewModel = new GoalFormViewModel
            {
                Goals = goals
            };

            if(goals.Count != 0)
            {
                return View(viewModel);
            }
            else
                return View();
        }

        public async Task<ActionResult> Save(GoalFormViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return RedirectToAction("TeamGoalsForm", model);
            }
            else
            {
                var user = await _userService.GetUser();
                foreach(var goal in model.Goals)
                {

                    if (goal.Id == null || goal.Id == 0)
                    {
                        goal.Team = user.Team;
                        _context.TeamGoals.Add(goal);
                    }
                    else
                    {
                        var goalToUpdate = _context.TeamGoals.SingleOrDefault(g => g.Id == goal.Id);

                        if(goal.Text == null)
                        {
                            _context.TeamGoals.Remove(goalToUpdate);
                        }
                        else
                        {
                            goalToUpdate.Text = goal.Text;
                            goalToUpdate.IsAchieved = goal.IsAchieved;
                        }
                    }
                }

                _context.SaveChanges();

                return Redirect("TeamHome");
            }
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
            var user = await _userService.GetUser();
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
                    VarsityRunners = _context.Runners.Where(r => r.Team.Id == user.Team.Id).Take(7).OrderBy(r => r.PersonalBest).ToList(),
                    NextRace = nextRace,
                    TeamGoals = _context.TeamGoals.Where(r => r.Team.Id == user.Team.Id).ToList()
                };
            }
            return View(teamHomeViewModel);
        }

        private Race CalculateNextRace(List<Race> teamRaceSchedule)
        {
            if(teamRaceSchedule.Count == 0)
            {
                return null;
            }
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