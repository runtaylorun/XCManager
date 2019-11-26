using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace XCManager.Models
{
    public class IsValidDate : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string date = value.ToString();
            DateTime dateValue;
            if(DateTime.TryParse(date, out dateValue))
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Date is not in the format MM/dd/yyyy");
            }
        }
    }
}