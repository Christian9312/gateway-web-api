using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gateways.Domain.Communication;
using Gateways.Domain.Contexts;
using Gateways.Domain.Contracts;

namespace Gateways.Domain.Services
{
    public class RepositoryWrapper:IRepositoryWrapper
    {
        private AppDbContext appDbContext;
        private IGatewayRepository gateway;
        private IPeripheralRepository peripheral;

        public RepositoryWrapper(AppDbContext context)
        {
            appDbContext = context;
        }

        public IGatewayRepository Gateway =>
            gateway ?? (gateway = new GatewayRepository(appDbContext));

        public IPeripheralRepository Peripheral =>
            peripheral ?? (peripheral = new PeripheralRepository(appDbContext));

        

        public async Task<Response> SaveAsync()
        {
            try
            {
                await appDbContext.SaveChangesAsync();
                return new Response(true,string.Empty);
            }
            catch (Exception e)
            {
                return new Response(false,e.Message);
            }
        }
    }
}
