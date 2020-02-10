using System;
using System.Collections.Generic;
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

        public RaceReportServices(ApplicationDbContext context, RaceService raceService, RunnerServices runnerService)
        {
            _context = context;
            _raceService = raceService;
            _runnerService = runnerService;
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
            RaceReport report = raceReport;
            report.Race = _raceService.GetRace(report.Race.Id.Value);

            CheckForPersonalBests(report);
            ProcessIndividualResults(report);


            var datestring = report.Race.Date.ToShortDateString();

            var path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/content/excelfiles/"),
                report.Race.RaceName + datestring.Replace('/', '-') + ".xlsx");

            ExcelDoc ex = new ExcelDoc();
            ex.CreateNewFile();
            for (int i = 0; i < report.Results.Count(); i++)
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
            binary.FileName = report.Race.RaceName + datestring.Replace('/', '-') + ".xlsx";

            var reportindb = _context.RaceReports.SingleOrDefault(r => r.FileName == binary.FileName);

            if (reportindb == null)
                _context.RaceReports.Add(binary);
            else
            {
                reportindb.FileName = binary.FileName;
                reportindb.Data = binary.Data;
            }

            _context.SaveChanges();

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

        private void CheckForPersonalBests(RaceReport report)
        {
            foreach (IndividualResult result in report.Results)
            {
                var currentRunner = _runnerService.GetRunner(result.Runner.Id.Value);
                if (currentRunner.PersonalBest.Ticks < result.FinishingTime.Ticks)
                {
                    currentRunner.PersonalBest = result.FinishingTime;
                }
            }
        }

        private void ProcessIndividualResults(RaceReport report)
        {
            foreach (IndividualResult result in report.Results)
            {
                result.Runner = _runnerService.GetRunner(result.Runner.Id.Value);
                result.Race = report.Race;
                _context.IndividualResults.Add(result);
            }
        }
    }
}