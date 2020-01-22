using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Gateways.Validators
{
    public class IpValidatorAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (!(value is string ip))
                return new ValidationResult("Invalid address value");

            var isValid = IPAddress.TryParse(ip, out _);

            return isValid ? ValidationResult.Success : new ValidationResult("Invalid address value");
        }
    }
}
