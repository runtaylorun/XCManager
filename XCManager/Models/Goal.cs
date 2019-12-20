using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace XCManager.Models
{
    public class Goal
    {
        public int? Id { get; set; }

        [StringLength(maximumLength: 120, ErrorMessage = "Goal text can not exceed 120 characters.")]
        [Display(Name = "Team Goal")]
        public string Text { get; set; }

        public Team Team { get; set; }
        public bool IsAchieved { get; set; }
    }
}