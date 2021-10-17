using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CheckOutTest.Data.Attributes
{
    public class ExpiryDateAnotation : ValidationAttribute
    {
        public ExpiryDateAnotation()
        {

        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || String.IsNullOrWhiteSpace(value.ToString()))
            {
                return new ValidationResult("Not a valid expiration date", new List<string> { validationContext.MemberName });
            }
            var expiryDate = value.ToString().Split('/');
            if (expiryDate.Length != 2)
            {
                return new ValidationResult("Not valid expiration date", new List<string> { validationContext.MemberName });
            }
            else
            {
                var month = Regex.Match(expiryDate[0], @"^[0-9]{2}$").Success;
                var year = Regex.Match(expiryDate[0], @"^[0-9]{2}$").Success;
                if (month && year)
                {
                    return ValidationResult.Success;
                }
                return new ValidationResult("Not valid expiration date", new List<string> { validationContext.MemberName });
            }
        }
    }
}
