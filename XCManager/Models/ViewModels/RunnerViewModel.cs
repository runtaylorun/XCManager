using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XCManager.Models;

namespace XCManager.Models.ViewModels
{
    public class RunnerViewModel
    {
        public Runner Runner { get; set; }
        public List<IndividualResult> RecentResults { get; set; }
        public Dictionary<String, TimeSpan> PersonalBests { get; set; }
    }
}