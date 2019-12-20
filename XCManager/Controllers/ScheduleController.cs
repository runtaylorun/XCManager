using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XCManager.Models;
using Microsoft.Office.Interop.Excel;
using XCManager.Excel;
using System.Threading.Tasks;
using System.IO;
using XCManager.Services;

namespace XCManager.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly IUserServices _userService;
        private ApplicationDbContext _context;

        public ScheduleController(IUserServices userService)
        {
            _context = new ApplicationDbContext();
            _userService = userService;
        }
        
        [Authorize]
        public async Task<ActionResult> Index()
        {
            var user = await _userService.GetUser();
            var races = _context.Races.OrderBy(r => r.Date).Where(r => r.Team.Id == user.Team.Id).ToList();
            return View(races);
        }

        [Authorize]
        public async Task<ActionResult> ReportForm(int id)
        {
            var user = await _userService.GetUser();
            var runners = _context.Runners.Where(r => r.Team.Id == user.Team.Id).ToList();
            var race = _context.Races.SingleOrDefault(r => r.Id == id);

            if (race == null)
                return HttpNotFound();

            var raceReport = new RaceReport
            {
                Results = new List<IndividualResult>(),
                Race = race
            };

            foreach(Runner runner in runners)
            {
                IndividualResult result = new IndividualResult
                {
                    Runner = runner
                };
                raceReport.Results.Add(result);
                
            }

            return View(raceReport);
        }

        [Authorize]
        public ActionResult NewRaceForm()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateReport(RaceReport Model)
        {
            RaceReport report = Model;
            report.Race = _context.Races.SingleOrDefault(r => r.Id == report.Race.Id);
            CheckForPersonalBests(report);
            foreach(IndividualResult result in report.Results)
            {
                result.Runner = _context.Runners.SingleOrDefault(r => r.Id == result.Runner.Id);
                result.Race = report.Race;
                _context.IndividualResults.Add(result);
            }
            var dateString = report.Race.Date.ToShortDateString();

            var path = Path.Combine(Server.MapPath("~/Content/ExcelFiles/"), 
                report.Race.RaceName + dateString.Replace('/', '-') + ".xlsx");

            ExcelDoc ex = new ExcelDoc();
            ex.CreateNewFile();
            for(int i = 0; i < report.Results.Count(); i++)
            {
                ex.WriteToCell(0, 0, "Name:");
                ex.WriteToCell(i + 1, 0, report.Results[i].Runner.Name);
            }
            for (int i = 0; i < report.Results.Count(); i++)
            {
                ex.WriteToCell(0, 1, "Time:");
                ex.WriteToCell(i + 1, 1, report.Results[i].FinishingTime.ToString());
            }
            for (int i = 0; i < report.Results.Count(); i++)
            {
                ex.WriteToCell(0, 2, "Place:");
                ex.WriteToCell(i + 1, 2, report.Results[i].Place.ToString());
            }
            ex.SaveAs(path);
            ex.Close();

            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            BinaryReader reader = new BinaryReader(fs);
            RaceReportBinary binary = new RaceReportBinary();
            binary.RaceId = report.Race.Id;

            binary.Data = reader.ReadBytes((Int32)fs.Length);
            binary.FileName = report.Race.RaceName + dateString.Replace('/', '-') + ".xlsx";

            var reportInDb = _context.RaceReports.SingleOrDefault(r => r.FileName == binary.FileName);

            if (reportInDb == null)
                _context.RaceReports.Add(binary);
            else
            {
                reportInDb.FileName = binary.FileName;
                reportInDb.Data = binary.Data;
            }

            _context.SaveChanges();

            fs.Close();
            reader.Close();

            System.IO.File.Delete(path);
           

            return RedirectToAction("Index");
        }

        public ActionResult DownloadReport(int id)
        {
            RaceReportBinary binary = _context.RaceReports.SingleOrDefault(r => r.RaceId == id);
            byte[] bytes = binary.Data;
            string fileName = binary.FileName;
            return File(bytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        public ActionResult DeleteRace(int id)
        {
            var raceInDb = _context.Races.SingleOrDefault(r => r.Id == id);

            _context.Races.Remove(raceInDb);
            _context.SaveChanges();

            return RedirectToAction("Index");

        }

        [HttpPost]
        public async Task<ActionResult> SaveRace(Race race)
        {
            if(!ModelState.IsValid)
            {
                foreach (ModelState modelState in ViewData.ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                    {
                        continue;
                    }
                }
                return RedirectToAction("NewRaceForm", race);
            }
            else
            {
                var user = await _userService.GetUser();
                race.Team = user.Team;
                if (race.Id == null || race.Id == 0)
                    _context.Races.Add(race);

                _context.SaveChanges();

                return RedirectToAction("Index");
            }
        }

        private void CheckForPersonalBests(RaceReport report)
        {
            foreach(IndividualResult result in report.Results)
            {
                var currentRunner = _context.Runners.SingleOrDefault(r => r.Id == result.Runner.Id);
                if(currentRunner.PersonalBest.Ticks < result.FinishingTime.Ticks)
                {
                    currentRunner.PersonalBest = result.FinishingTime;
                }
            }
        }
    }
}