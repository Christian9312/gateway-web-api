using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gateways.Domain.Contexts;
using Gateways.Domain.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Gateways.Domain.Services
{
    public class GatewayRepository:RepositoryBase<Gateway>, IGatewayRepository
    {
        public GatewayRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Gateway>> GetAllGateways()
        {
            return await FindAll().OrderBy(gateway => gateway.Name).Include(x => x.AssociatedPeripherals).ToListAsync();
        }

        public async Task<Gateway> GetGatewayById(string serialNumber)
        {
            return await FindByCondition(gateway => gateway.SerialNumber == serialNumber).Include(x => x.AssociatedPeripherals)
                .FirstOrDefaultAsync();
        }

    }
}
