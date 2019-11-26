using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace XCManager.Models
{
    public class Race
    {
        public int? Id { get; set; }

        [Required]
        [MaxLength(length: 55, ErrorMessage = "Race name cannot be longer than 55 characters")]
        [Display(Name = "Race Name")]
        public string RaceName { get; set; }

        [Required]
        [IsValidDate]
        public DateTime Date { get; set; }

        [Required]
        [MaxLength(length: 15, ErrorMessage = "Distance cannot be longer than 15 characters")]
        public string Distance { get; set; }

        [Required]
        [MaxLength(length: 50, ErrorMessage = "Location cannot be longer than 50 characters")]
        public string Location { get; set; }

        public Team Team { get; set; }
    }
}