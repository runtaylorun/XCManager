using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Office.Interop.Excel;
using XCManager.Excel;
using XCManager.Models;


namespace XCManager.Services
{
    public interface IRaceReportService
    {
        void GenerateReport(RaceReport report);
        Task<RaceReport> CreateEmptyRaceReport(int raceId);
        void ProcessRaceReport(RaceReport raceReport);
        ExcelFile GetRaceReportExcelFile(int raceId);
    }

    public class RaceReportServices : IRaceReportService
    {
        private readonly ApplicationDbContext _context;
        private readonly RaceService _raceService;
        private readonly RunnerServices _runnerService;
        private readonly UserServices _userService;

        public RaceReportServices(ApplicationDbContext context, RaceService raceService, RunnerServices runnerService, UserServices userService)
        {
            _context = context;
            _raceService = raceService;
            _runnerService = runnerService;
            _userService = userService;
        }

        public void GenerateReport(RaceReport report)
        {
            report.Race = _raceService.GetRace(report.Race.Id.Value);
            CheckForPersonalBests(report);
        }

        public async Task<RaceReport> CreateEmptyRaceReport(int raceId)
        {

            var raceReport = new RaceReport
            {
                Results = new List<IndividualResult>(),
                Race = _raceService.GetRace(raceId)
            };

            var runners = await _runnerService.GetTeamRoster();

            foreach (var runner in runners)
            {
                IndividualResult result = new IndividualResult
                {
                    Runner = runner
                };
                raceReport.Results.Add(result);

            }

            return raceReport;
        }

        public void ProcessRaceReport(RaceReport raceReport)
        {
            var currentReportsRace = _raceService.GetRace(raceReport.Race.Id.Value);
            _context.Races.Attach(currentReportsRace);
            raceReport.Race = currentReportsRace;

            CheckForPersonalBests(raceReport);
            ProcessIndividualResults(raceReport);

            var datestring = currentReportsRace.Date.ToShortDateString();

            var path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/content/excelfiles/"),
                currentReportsRace.RaceName + datestring.Replace('/', '-') + ".xlsx");

            ExcelDoc ex = new ExcelDoc();
            ex.CreateNewFile();

            WriteNameColumn(ex, raceReport);
            WriteTimeColumn(ex, raceReport);
            WritePlaceColumn(ex, raceReport);
            
            ex.SaveAs(path);
            ex.Close();

            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            BinaryReader reader = new BinaryReader(fs);
            RaceReportBinary binary = new RaceReportBinary
            {
                RaceId = raceReport.Race.Id,

                Data = reader.ReadBytes((Int32)fs.Length),
                FileName = currentReportsRace.RaceName + datestring.Replace('/', '-') + ".xlsx"
            };

            var reportindb = _context.RaceReports.SingleOrDefault(r => r.FileName == binary.FileName);

            if (reportindb == null)
                _context.RaceReports.Add(binary);
            else
            {
                reportindb.FileName = binary.FileName;
                reportindb.Data = binary.Data;
            }

            try
            {
                _context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    System.Diagnostics.Debug.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        System.Diagnostics.Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }

            fs.Close();
            reader.Close();

            System.IO.File.Delete(path);
        }

        public ExcelFile GetRaceReportExcelFile(int raceId)
        {
            var reportBinary = _context.RaceReports.SingleOrDefault(r => r.RaceId == raceId);

            ExcelFile file = new ExcelFile
            {
                Bytes = reportBinary.Data,
                FileName = reportBinary.FileName
            };

            return file;
        }

        private void WriteNameColumn(ExcelDoc doc, RaceReport report)
        {
            for (int i = 0; i < report.Results.Count(); i++)
            {
                var currentRunnerId = report.Results[i].Runner.Id;
                var currentRunner = _context.Runners.SingleOrDefault(r => r.Id == currentRunnerId);
                doc.WriteToCell(0, 0, "Name:");
                doc.WriteToCell(i + 1, 0, currentRunner.Name);
            }
        }

        private void WriteTimeColumn(ExcelDoc doc, RaceReport report)
        {
            for (int i = 0; i < report.Results.Count(); i++)
            {
                doc.WriteToCell(0, 1, "Time:");
                doc.WriteToCell(i + 1, 1, report.Results[i].FinishingTime.ToString());
            }
        }

        private void WritePlaceColumn(ExcelDoc doc, RaceReport report)
        {
            for (int i = 0; i < report.Results.Count(); i++)
            {
                doc.WriteToCell(0, 2, "Place:");
                doc.WriteToCell(i + 1, 2, report.Results[i].Place.ToString());
            }
        }

        private void CheckForPersonalBests(RaceReport report)
        {
            foreach (IndividualResult result in report.Results)
            {
                var currentRunner = _runnerService.GetRunner(result.Runner.Id.Value);
                result.Runner = currentRunner;
                if (currentRunner.PersonalBest.Ticks < result.FinishingTime.Ticks)
                {
                    _context.Runners.Attach(currentRunner);
                    currentRunner.PersonalBest = result.FinishingTime;
                    _context.Entry(currentRunner).State = System.Data.Entity.EntityState.Modified;
                }
            }
        }

        private void ProcessIndividualResults(RaceReport report)
        {
            foreach (IndividualResult result in report.Results)
            {
                result.Race = report.Race;
                _context.IndividualResults.Add(result);
            }
        }
    }
}