using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gateways.Domain.Resources
{
    public class PeripheralSimpleDto
    {
        [Required(ErrorMessage = "UID is required")]
        public uint UId { get; set; }

        [StringLength(60, ErrorMessage = "Vendor can't be longer than 60 characters")]
        public string Vendor { get; set; }

        [Required(ErrorMessage = "Date created is required")]
        public DateTimeOffset? CreationDate { get; set; }

        [Required(ErrorMessage = "Status is required")]
        public string Status { get; set; }
    }
}
