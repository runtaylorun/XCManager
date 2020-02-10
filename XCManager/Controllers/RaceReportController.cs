using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.Office.Interop;
using XCManager.Models;
using XCManager.Services;

namespace XCManager.Controllers
{
    public class RaceReportController : Controller
    {

        private readonly IUserServices _userService;
        private readonly IRaceService _raceService;
        private readonly IRunnerServices _runnerService;
        private readonly IRaceReportService _raceReportService;

        public RaceReportController(IUserServices userService, IRaceService raceService, IRunnerServices runnerService, IRaceReportService raceReportService)
        {
            _userService = userService;
            _raceService = raceService;
            _runnerService = runnerService;
            _raceReportService = raceReportService;
        }

        [Authorize]
        public async Task<ActionResult> ReportForm(int id)
        {
            var raceReport = await _raceReportService.CreateEmptyRaceReport(id);

            return View(raceReport);
        }


        [HttpPost]
        public ActionResult createreport(RaceReport model)
        {
            _raceReportService.ProcessRaceReport(model);


            return RedirectToAction("Index", "Schedule");
        }

        public ActionResult DownloadReport(int id)
        {
            var file = _raceReportService.GetRaceReportExcelFile(id);

            return File(file.Bytes, System.Net.Mime.MediaTypeNames.Application.Octet, file.FileName);
        }
    }
}