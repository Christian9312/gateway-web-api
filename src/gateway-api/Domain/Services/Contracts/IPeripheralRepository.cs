using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateways.Domain.Contracts
{
    public interface IPeripheralRepository: IRepositoryBase<Peripheral>
    {
        Task<IEnumerable<Peripheral>> GetAllPeripherals();

        Task<Peripheral> GetPeripheralById(uint id);
        Task<IEnumerable<Peripheral>> GetPeripheralByGateways(string gatewayNumber);

    }
}
