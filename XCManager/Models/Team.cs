using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace XCManager.Models
{
    public class Team
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        [Display(Name = "Coach Name")]
        public string CoachName { get; set; }
    }
}