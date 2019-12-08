using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XCManager.Models.ViewModels
{
    public class TeamHomeViewModel
    {
        public Team Team { get; set; }
        public List<Runner> VarsityRunners { get; set; }
        public Race NextRace { get; set; }
    }
}