using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Office.Interop;
using XCManager.Models;

namespace XCManager.Controllers
{
    public class RaceReportController : Controller
    {

        //[Authorize]
        //public async Task<ActionResult> ReportForm(int id)
        //{
        //    var user = await _userService.GetUser();
        //    var runners = _context.Runners.Where(r => r.Team.Id == user.Team.Id).ToList();
        //    var race = _raceService.GetRace(id);

        //    if (race == null)
        //        return HttpNotFound();

        //    var raceReport = new RaceReport
        //    {
        //        Results = new List<IndividualResult>(),
        //        Race = race
        //    };

        //    foreach (Runner runner in runners)
        //    {
        //        IndividualResult result = new IndividualResult
        //        {
        //            Runner = runner
        //        };
        //        raceReport.Results.Add(result);

        //    }

        //    return View(raceReport);
        //}


        //[HttpPost]
        //public ActionResult CreateReport(RaceReport Model)
        //{
        //    RaceReport report = Model;
        //    report.Race = _context.Races.SingleOrDefault(r => r.Id == report.Race.Id);
        //    CheckForPersonalBests(report);
        //    foreach (IndividualResult result in report.Results)
        //    {
        //        result.Runner = _context.Runners.SingleOrDefault(r => r.Id == result.Runner.Id);
        //        result.Race = report.Race;
        //        _context.IndividualResults.Add(result);
        //    }
        //    var dateString = report.Race.Date.ToShortDateString();

        //    var path = Path.Combine(Server.MapPath("~/Content/ExcelFiles/"),
        //        report.Race.RaceName + dateString.Replace('/', '-') + ".xlsx");

        //    ExcelDoc ex = new ExcelDoc();
        //    ex.CreateNewFile();
        //    for (int i = 0; i < report.Results.Count(); i++)
        //    {
        //        ex.WriteToCell(0, 0, "Name:");
        //        ex.WriteToCell(i + 1, 0, report.Results[i].Runner.Name);
        //    }
        //    for (int i = 0; i < report.Results.Count(); i++)
        //    {
        //        ex.WriteToCell(0, 1, "Time:");
        //        ex.WriteToCell(i + 1, 1, report.Results[i].FinishingTime.ToString());
        //    }
        //    for (int i = 0; i < report.Results.Count(); i++)
        //    {
        //        ex.WriteToCell(0, 2, "Place:");
        //        ex.WriteToCell(i + 1, 2, report.Results[i].Place.ToString());
        //    }
        //    ex.SaveAs(path);
        //    ex.Close();

        //    FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
        //    BinaryReader reader = new BinaryReader(fs);
        //    RaceReportBinary binary = new RaceReportBinary();
        //    binary.RaceId = report.Race.Id;

        //    binary.Data = reader.ReadBytes((Int32)fs.Length);
        //    binary.FileName = report.Race.RaceName + dateString.Replace('/', '-') + ".xlsx";

        //    var reportInDb = _context.RaceReports.SingleOrDefault(r => r.FileName == binary.FileName);

        //    if (reportInDb == null)
        //        _context.RaceReports.Add(binary);
        //    else
        //    {
        //        reportInDb.FileName = binary.FileName;
        //        reportInDb.Data = binary.Data;
        //    }

        //    _context.SaveChanges();

        //    fs.Close();
        //    reader.Close();

        //    System.IO.File.Delete(path);


        //    return RedirectToAction("Index");
        //}

        //public ActionResult DownloadReport(int id)
        //{
        //    RaceReportBinary binary = _context.RaceReports.SingleOrDefault(r => r.RaceId == id);
        //    byte[] bytes = binary.Data;
        //    string fileName = binary.FileName;
        //    return File(bytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        //}
    }
}