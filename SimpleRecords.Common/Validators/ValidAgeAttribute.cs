using System;
using System.ComponentModel.DataAnnotations;

namespace SimpleRecords.Common.Validators
{
    public class ValidAgeAttribute : ValidationAttribute
    {
        public ValidAgeAttribute()
            : base("{0} value does not lead to a valid age.")
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime val = (DateTime)value;

            bool valid = val.Year >= 1920;

            if (valid)
                return null;

            // allows for unit testing proper operation of of ther attribute
            if (validationContext == null)
                validationContext = new ValidationContext(val);

            return new ValidationResult(base.FormatErrorMessage(validationContext.MemberName)
                , new string[] { validationContext.MemberName });
        }
    }
}
