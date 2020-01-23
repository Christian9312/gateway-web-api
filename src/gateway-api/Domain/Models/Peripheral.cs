using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Gateways.Domain
{
    public class Peripheral
    {
        [Required(ErrorMessage = "UID is required")]
        public uint UId { get; set; }

        [StringLength(60, ErrorMessage = "Vendor can't be longer than 60 characters")]
        public string Vendor { get; set; }

        [Required(ErrorMessage = "Date created is required")]
        public DateTimeOffset CreationDate { get; set; }

        [Required(ErrorMessage = "Status is required")]
        public PeripheralStatus Status { get; set; }

        public string GatewayId { get; set; }
        public Gateway Gateway { get; set; }


    }

    public enum PeripheralStatus
    {

        [Description("Offline")]
        Offline,

        [Description("Online")]
        Online
    }
}
