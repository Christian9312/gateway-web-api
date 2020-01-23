using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gateways.Domain.Contexts;
using Gateways.Domain.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Gateways.Domain.Services
{
    public class PeripheralRepository:RepositoryBase<Peripheral>, IPeripheralRepository
    {
        public PeripheralRepository(AppDbContext context) : base(context)
        {

        }

        public async Task<IEnumerable<Peripheral>> GetAllPeripherals()
        {
            return await FindAll().OrderBy(peripheral => peripheral.Vendor).Include(x=> x.Gateway).ToListAsync();
        }

        public async Task<Peripheral> GetPeripheralById(uint id)
        {
            return await FindByCondition(gateway => gateway.UId == id).Include(x=> x.Gateway)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Peripheral>> GetPeripheralByGateways(string gatewayNumber)
        {
            return await FindByCondition(peripheral => peripheral.GatewayId == gatewayNumber).ToListAsync();
        }
    }
}
