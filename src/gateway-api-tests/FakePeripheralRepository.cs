using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Gateways.Domain;
using Gateways.Domain.Contracts;

namespace gateway_api_tests
{
    public class FakePeripheralRepository:IPeripheralRepository
    {
        private Dictionary<int, Peripheral> DbContext { get; } = new Dictionary<int, Peripheral>
        {
            {323123, new Peripheral{} },
            {4343443, new Peripheral{} },
            {5454354, new Peripheral{}},
            {5667677, new Peripheral{}}
        };

        public IQueryable<Peripheral> FindAll()
        {
            return new EnumerableQuery<Peripheral>(DbContext.Values.ToList());
        }

        public IQueryable<Peripheral> FindByCondition(Expression<Func<Peripheral, bool>> expression)
        {
            var selectedItems = DbContext.Select(keyValuePair => keyValuePair.Value)
                .Where(expression.Compile());
            return new EnumerableQuery<Peripheral>(selectedItems);
        }

        public void Create(Peripheral entity)
        {
            if (!DbContext.ContainsKey(entity.UId))
                DbContext.Add(entity.UId, entity);
        }

        public void Update(Peripheral entity)
        {
            DbContext[entity.UId] = entity;
        }

        public void Delete(Peripheral entity)
        {
            DbContext.Remove(entity.UId);
        }

        public async Task<IEnumerable<Peripheral>> GetAllPeripherals()
        {
            return  await Task.FromResult(FindAll().OrderBy(peripheral => peripheral.Vendor));
        }

        public async Task<Peripheral> GetPeripheralById(int id)
        {
            return await Task.FromResult(FindByCondition(peripheral => peripheral.UId == id).FirstOrDefault()) ;
        }

        public async Task<IEnumerable<Peripheral>> GetPeripheralByGateways(string gatewayNumber)
        {
            return await Task.FromResult(FindByCondition(peripheral => peripheral.GatewayId == gatewayNumber));
        }
    }
}
