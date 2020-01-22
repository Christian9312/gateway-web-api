using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateways.Domain.Contracts
{
    public interface IGatewayRepository: IRepositoryBase<Gateway>
    {
        Task<IEnumerable<Gateway>> GetAllGateways();

        Task<Gateway> GetGatewayById(string serialNumber);

    }
}
