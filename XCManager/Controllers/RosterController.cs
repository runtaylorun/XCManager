using System.Web.Mvc;
using XCManager.Models;
using XCManager.Models.ViewModels;
using System.Threading.Tasks;
using XCManager.Services;

namespace XCManager.Controllers
{
    public class RosterController : Controller
    {
        private readonly IRunnerServices _runnerService;

        public RosterController(IRunnerServices runnerService)
        {
            _runnerService = runnerService;
        }

        public async Task<ActionResult> Index()
        {
            RosterViewModel runners = new RosterViewModel
            {
                Runners = await _runnerService.GetTeamRoster()
            };

            return View(runners);
        }

        public ActionResult NewRunner()
        {
            return View("RunnerForm");
        }

        public ActionResult UpdateRunner(int id)
        {
            return View("RunnerForm", _runnerService.GetRunner(id));
        }

        public ActionResult DeleteRunner(int id)
        {
            _runnerService.DeleteRunner(id);
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Save(Runner runner)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("NewRunner", runner);
            }
            else
            {
                await _runnerService.PostRunner(runner);
                return RedirectToAction("Index");
            }
            
        }

        public ActionResult RunnerDetails(int id)
        {
            RunnerViewModel runner = new RunnerViewModel
            {
                Runner = _runnerService.GetRunner(id),
                RecentResults = _runnerService.GetRunnersRecentResults(id),
                PersonalBests = _runnerService.GetRunnersPersonalBests(id)
            };
            
            return View(runner);
        }
    }
}