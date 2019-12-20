using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Office.Interop.Excel;
using XCManager.Excel;
using XCManager.Models;

namespace XCManager.Services
{
    public class RaceReportServices
    {
        private readonly ApplicationDbContext _context;

        public RaceReportServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public void GenerateReport(RaceReport report)
        {
            report.Race = _context.Races.SingleOrDefault(r => r.Id == report.Race.Id);
            CheckForPersonalBests(report);
        }

        private void CheckForPersonalBests(RaceReport report)
        {
            foreach (IndividualResult result in report.Results)
            {
                var currentRunner = _context.Runners.SingleOrDefault(r => r.Id == result.Runner.Id);
                if (currentRunner.PersonalBest.Ticks < result.FinishingTime.Ticks)
                {
                    currentRunner.PersonalBest = result.FinishingTime;
                }
            }
        }
    }
}