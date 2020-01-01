using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XCManager.Models
{
    public class Runner
    {
        private string _phonenumber;
        private string _email;

        public int? Id { get; set; }

        [Required]
        [StringLength(maximumLength: 40, ErrorMessage = "Name length must not exceed 40 characters")]
        public string Name { get; set; }

        [Required]
        [RegularExpression(@"([0-9]{1,2})", ErrorMessage = "Not a valid grade")]
        public byte Grade { get; set; }

        [Display(Name = "Email*")]
        [StringLength(maximumLength: 50, ErrorMessage = "Email length must not exceed 50 characters")]
        [DataType(DataType.EmailAddress)]
        public string Email
        {
            get
            {
                if (_email == null || _email == "")
                    return "N/A";
                else
                    return _email;
            }
            set
            {
                _email = value;
            }
        }

        [Display(Name = "Phone Number*")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(maximumLength: 14, ErrorMessage = "Phone number must not exceed 14 characters")]
        public string PhoneNumber
        {
            get
            {
                if (_phonenumber == null || _phonenumber == "")
                    return "N/A";
                else
                    return _phonenumber;
            }
            set
            {
                _phonenumber = value;
            }
        }

        public Team Team { get; set; }
        public TimeSpan PersonalBest { get; set; }

    }
}