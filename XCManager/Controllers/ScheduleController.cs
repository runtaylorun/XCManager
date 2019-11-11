using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XCManager.Models;
using Microsoft.Office.Interop.Excel;
using XCManager.Excel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using System.IO;

namespace XCManager.Controllers
{
    public class ScheduleController : Controller
    {
        private ApplicationDbContext _context;
        private UserManager<ApplicationUser> UserManager { get; set; }

        public ScheduleController()
        {
            _context = new ApplicationDbContext();
            UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
        }
        // GET: Schedule
        [Authorize]
        public async Task<ActionResult> Index()
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            var races = _context.Races.OrderBy(r => r.Date).Where(r => r.Team.Id == user.Team.Id).ToList();
            return View(races);
        }

        [Authorize]
        public async Task<ActionResult> ReportForm(int id)
        {
            var user = await UserManager.FindByNameAsync(User.Identity.GetUserName());
            var runners = _context.Runners.Where(r => r.Team.Id == user.Team.Id).ToList();
            var race = _context.Races.SingleOrDefault(r => r.Id == id);

            if (race == null)
                return HttpNotFound();

            var raceReport = new RaceReport
            {
                results = new List<IndividualResult>(),
                race = race
            };

            foreach(Runner runner in runners)
            {
                IndividualResult result = new IndividualResult
                {
                    runner = runner
                };
                raceReport.results.Add(result);
                
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
            report.race = _context.Races.SingleOrDefault(r => r.Id == report.race.Id);
            foreach(IndividualResult result in report.results)
            {
                result.runner = _context.Runners.SingleOrDefault(r => r.Id == result.runner.Id);
                result.Race = report.race;
                _context.IndividualResults.Add(result);
            }
            var dateString = report.race.Date.ToShortDateString();

            var path = Path.Combine(Server.MapPath("~/Content/ExcelFiles/"), 
                report.race.RaceName + dateString.Replace('/', '-') + ".xlsx");

            ExcelDoc ex = new ExcelDoc();
            ex.CreateNewFile();
            for(int i = 0; i < report.results.Count(); i++)
            {
                ex.WriteToCell(0, 0, "Name:");
                ex.WriteToCell(i + 1, 0, report.results[i].runner.Name);
            }
            for (int i = 0; i < report.results.Count(); i++)
            {
                ex.WriteToCell(0, 1, "Time:");
                ex.WriteToCell(i + 1, 1, report.results[i].finishingTime.ToString());
            }
            for (int i = 0; i < report.results.Count(); i++)
            {
                ex.WriteToCell(0, 2, "Place:");
                ex.WriteToCell(i + 1, 2, report.results[i].Place.ToString());
            }
            ex.SaveAs(path);
            ex.Close();

            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            BinaryReader reader = new BinaryReader(fs);
            RaceReportBinary binary = new RaceReportBinary();
            binary.RaceId = report.race.Id;

            binary.Data = reader.ReadBytes((Int32)fs.Length);
            binary.FileName = report.race.RaceName + dateString.Replace('/', '-') + ".xlsx";

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
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            race.Team = user.Team;
            if (race.Id == 0)
                _context.Races.Add(race);

            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}