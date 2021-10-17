using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
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
                DateTime.TryParseExact(value.ToString(), "MM/yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime expiryDate1);
                if (expiryDate1 != null && expiryDate1 > DateTime.Now)
                {
                    return ValidationResult.Success;
                }
                return new ValidationResult("Not valid expiration date", new List<string> { validationContext.MemberName });
            }
        }
    }
}
