using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XCManager.Models;

namespace XCManager.Models
{
    public class RaceReport
    {
        public int ReportId { get; set; }
        public List<IndividualResult> results { get; set; }
        public Race race { get; set; }
    }
}