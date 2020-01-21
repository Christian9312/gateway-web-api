using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        public IList<Peripheral> AssociatedPeripherals { get; set; } = new List<Peripheral>();

    }
}
