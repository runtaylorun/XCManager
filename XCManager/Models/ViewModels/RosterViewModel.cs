using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XCManager.Models;

namespace XCManager.Models.ViewModels
{
    public class RosterViewModel
    {
        public List<Runner> Runners { get; set; }
        public bool IsRunnersListEmpty
        {
            get { return Runners.Count == 0; }
        }
    }
}