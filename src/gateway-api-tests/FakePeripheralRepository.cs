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
        private Dictionary<uint, Peripheral> DbContext { get; } = new Dictionary<uint, Peripheral>
        {
            {3231232, new Peripheral{UId =3231232, Vendor = "My Enterprise", Status = PeripheralStatus.Online, CreationDate = DateTimeOffset.Now,GatewayId = "4451-9834-7885-3446"}},
            {4343443, new Peripheral{UId =4343443, Vendor = "Your Enterprise", Status = PeripheralStatus.Offline, CreationDate = DateTimeOffset.Now,GatewayId = "8763-3242-3343-8898"}},
            {5454354, new Peripheral{UId =5454354, Vendor = "Their Enterprise", Status = PeripheralStatus.Online, CreationDate = DateTimeOffset.Now,GatewayId = "5654-5653-3423-6675"}},
            {5667677, new Peripheral{UId =5667677, Vendor = "Enterprise SA", Status = PeripheralStatus.Offline, CreationDate = DateTimeOffset.Now,GatewayId = "8763-3242-3343-8898"}},
            {7857677, new Peripheral{UId =7857677, Vendor = "Enterprise SA", Status = PeripheralStatus.Online, CreationDate = DateTimeOffset.Now,GatewayId = "8763-3242-3343-8898"}},
            {4217547, new Peripheral{UId =4217547, Vendor = "Enterprise SA", Status = PeripheralStatus.Online, CreationDate = DateTimeOffset.Now,GatewayId = "8763-3242-3343-8898"}},
            {0866527, new Peripheral{UId =0866527, Vendor = "Your Enterprise", Status = PeripheralStatus.Online, CreationDate = DateTimeOffset.Now,GatewayId = "8763-3242-3343-8898"}},
            {1367657, new Peripheral{UId =1367657, Vendor = "Your Enterprise", Status = PeripheralStatus.Offline, CreationDate = DateTimeOffset.Now,GatewayId = "8763-3242-3343-8898"}},
            {3466767, new Peripheral{UId =3466767, Vendor = "My Enterprise", Status = PeripheralStatus.Online, CreationDate = DateTimeOffset.Now,GatewayId = "8763-3242-3343-8898"}},
            {3733673, new Peripheral{UId =3733673, Vendor = "My Enterprise", Status = PeripheralStatus.Online, CreationDate = DateTimeOffset.Now,GatewayId = "8763-3242-3343-8898"}},
            {1233677, new Peripheral{UId =1233677, Vendor = "Their Enterprise", Status = PeripheralStatus.Offline, CreationDate = DateTimeOffset.Now,GatewayId = "8763-3242-3343-8898"}},
            {5434677, new Peripheral{UId =5434677, Vendor = "Their Enterprise", Status = PeripheralStatus.Offline, CreationDate = DateTimeOffset.Now,GatewayId = "8763-3242-3343-8898"}}
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
           var id = (uint) new Random().Next(0, maxValue:int.MaxValue);
;          DbContext.Add(id, entity);
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

        public async Task<Peripheral> GetPeripheralById(uint id)
        {
            return await Task.FromResult(FindByCondition(peripheral => peripheral.UId == id).FirstOrDefault()) ;
        }

        public async Task<IEnumerable<Peripheral>> GetPeripheralByGateways(string gatewayNumber)
        {
            return await Task.FromResult(FindByCondition(peripheral => peripheral.GatewayId == gatewayNumber));
        }
    }
}
