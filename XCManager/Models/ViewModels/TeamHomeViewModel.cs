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
        public List<Goal> TeamGoals { get; set; }

        public bool IsTeamGoalsEmpty
        {
            get { return TeamGoals.Count == 0; }
        }

        public bool IsVarsityRunnersEmpty
        {
            get { return VarsityRunners.Count == 0; }
        }

        public bool IsNextRaceEmpty
        {
            get { return NextRace == null; }
        }

        public int DaysTillNextRace
        {
            get { return NextRace.Date.Subtract(DateTime.Now).Days; }
        }
    }
}