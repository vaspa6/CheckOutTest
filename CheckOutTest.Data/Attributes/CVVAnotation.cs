using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CheckOutTest.Data.Attributes
{
    public class CVVAnotation : ValidationAttribute
    {
        public CVVAnotation()
        {

        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult("CVV must be a valid 3 digit number (4 for Amex)", new List<string> { validationContext.MemberName });
            }
            if (value is int)
            {
                var cvv = Convert.ToInt32(value);
                if (cvv <= 0)
                {
                    return new ValidationResult("CVV must be a valid 3 digit number (4 for Amex)", new List<string> { validationContext.MemberName });
                }
                if ((int)Math.Floor(Math.Log10(cvv)) + 1 != 3 && (int) Math.Floor(Math.Log10(cvv)) + 1 != 4)
                {
                    return new ValidationResult("CVV must be a valid 3 digit number (4 for Amex)", new List<string> { validationContext.MemberName });
                }
                return ValidationResult.Success;
            }
            return new ValidationResult("CVV must be a valid 3 digit number (4 for Amex)", new List<string> { validationContext.MemberName });
        }
    }
}
