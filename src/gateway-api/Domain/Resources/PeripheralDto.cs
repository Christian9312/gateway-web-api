﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Gateways.Domain.Resources
{
    public class PeripheralDto
    {
        [StringLength(60, ErrorMessage = "Vendor can't be longer than 60 characters")]
        public string Vendor { get; set; }

        [Required(ErrorMessage = "Date created is required")]
        public DateTimeOffset? CreationDate { get; set; }

        [Required(ErrorMessage = "Status is required")]
        public PeripheralStatus? Status { get; set; }

        [Required]
        public string GatewayId { get; set; }
    }
}
