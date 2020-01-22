using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Gateways.Domain.Communication;
using Gateways.Domain.Contracts;

namespace gateway_api_tests
{
    public class FakeRepositoryWrapper:IRepositoryWrapper
    {
        private IGatewayRepository gateway;
        private IPeripheralRepository peripheral;

        public FakeRepositoryWrapper()
        { }

        public IGatewayRepository Gateway =>
            gateway ?? (gateway = new FakeGatewayRepository());

        public IPeripheralRepository Peripheral =>
            peripheral ?? (peripheral = new FakePeripheralRepository());
        public async Task<Response> SaveAsync()
        {
            return await Task.FromResult(new Response(true, string.Empty));
        }
    }
}
