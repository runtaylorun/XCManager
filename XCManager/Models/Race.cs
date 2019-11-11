using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace XCManager.Models
{
    public class Race
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Race Name")]
        public string RaceName { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string Distance { get; set; }

        [Required]
        public string Location { get; set; }

        public Team Team { get; set; }
    }
}