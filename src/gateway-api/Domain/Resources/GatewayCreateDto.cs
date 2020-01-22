using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Gateways.Validators;

namespace Gateways.Domain.Resources
{
    public class GatewayCreateDto
    {
        [Required(ErrorMessage = "Serial number is required")]
        public string SerialNumber { get; set; }

        [StringLength(60, ErrorMessage = "Name can't be longer than 60 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [IpValidator]
        public string Address { get; set; }
    }
}
