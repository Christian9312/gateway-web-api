using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateways.Domain
{
    public class Peripheral
    {
        public int UId { get; set; }

        public string Vendor { get; set; }

        public DateTimeOffset CreationDate { get; set; }

        public PeripheralStatus Status { get; set; }

    }

    public enum PeripheralStatus
    {
        Offline,
        Online
    }
}
