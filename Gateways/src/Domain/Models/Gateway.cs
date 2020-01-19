using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace Gateways.Domain
{
    public class Gateway
    {
        public string SerialNumber { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public IEnumerable<Peripheral> AssociatedPeripherals { get; set; }

    }
}
