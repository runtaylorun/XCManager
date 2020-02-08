using System.Web.Mvc;
using XCManager.Models;
using System.Threading.Tasks;
using XCManager.Services;
using System.Collections.Generic;

namespace XCManager.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly IUserServices _userService;
        private readonly IRaceService _raceService;

        public ScheduleController(IUserServices userService, IRaceService raceService)
        {
            _userService = userService;
            _raceService = raceService;
        }
        
        [Authorize]
        public async Task<ActionResult> Index()
        {
            IEnumerable<Race> races = await _raceService.GetRaces();
            return View(races);
        }

        [Authorize]
        public ActionResult NewRaceForm()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> SaveRace(Race race)
        {
            if(!ModelState.IsValid)
            {
                return RedirectToAction("NewRaceForm", race);
            }
            else
            {
                await _raceService.SaveRace(race);
   
                return RedirectToAction("Index");
            }
        }
    }
}