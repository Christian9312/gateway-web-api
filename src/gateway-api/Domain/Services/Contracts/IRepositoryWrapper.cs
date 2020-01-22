using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gateways.Domain.Communication;

namespace Gateways.Domain.Contracts
{
    public interface IRepositoryWrapper
    {
        IGatewayRepository Gateway { get; }
        IPeripheralRepository Peripheral { get; }
        Task<Response> SaveAsync();
    }
}
