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
    public class MockGatewayRepository : IGatewayRepository
    {
        private Dictionary<string, Gateway> DbContext { get; } = new Dictionary<string, Gateway>
        {
            {"6521-1434-3451-4531", new Gateway{SerialNumber = "6521-1434-3451-4531", Name = "Network 1",Address = "192.168.1.1", AssociatedPeripherals = new List<Peripheral>()} },
            {"4451-9834-7885-3446", new Gateway{SerialNumber = "4451-9834-7885-3446", Name = "Network 2",Address = "192.168.1.2", AssociatedPeripherals = new List<Peripheral>()} },
            {"5654-5653-3423-6675", new Gateway{SerialNumber = "5654-5653-3423-6675", Name = "Network 3",Address = "192.168.1.3", AssociatedPeripherals = new List<Peripheral>()} },
            {"8763-3242-3343-8898", new Gateway{SerialNumber = "8763-3242-3343-8898", Name = "Network 4",Address = "192.168.1.4", AssociatedPeripherals = new List<Peripheral>
            {
                new Peripheral{UId =5667677, Vendor = "Enterprise SA", Status = PeripheralStatus.Offline, CreationDate = DateTimeOffset.Now,GatewayId = "8763-3242-3343-8898"},
                new Peripheral{UId =7857677, Vendor = "Enterprise SA", Status = PeripheralStatus.Online, CreationDate = DateTimeOffset.Now,GatewayId = "8763-3242-3343-8898"},
                new Peripheral{UId =4217547, Vendor = "Enterprise SA", Status = PeripheralStatus.Online, CreationDate = DateTimeOffset.Now,GatewayId = "8763-3242-3343-8898"},
                new Peripheral{UId =0866527, Vendor = "Your Enterprise", Status = PeripheralStatus.Online, CreationDate = DateTimeOffset.Now,GatewayId = "8763-3242-3343-8898"},
                new Peripheral{UId =1367657, Vendor = "Your Enterprise", Status = PeripheralStatus.Offline, CreationDate = DateTimeOffset.Now,GatewayId = "8763-3242-3343-8898"},
                new Peripheral{UId =3466767, Vendor = "My Enterprise", Status = PeripheralStatus.Online, CreationDate = DateTimeOffset.Now,GatewayId = "8763-3242-3343-8898"},
                new Peripheral{UId =3733673, Vendor = "My Enterprise", Status = PeripheralStatus.Online, CreationDate = DateTimeOffset.Now,GatewayId = "8763-3242-3343-8898"},
                new Peripheral{UId =1233677, Vendor = "Their Enterprise", Status = PeripheralStatus.Offline, CreationDate = DateTimeOffset.Now,GatewayId = "8763-3242-3343-8898"},
                new Peripheral{UId =5434677, Vendor = "Their Enterprise", Status = PeripheralStatus.Offline, CreationDate = DateTimeOffset.Now,GatewayId = "8763-3242-3343-8898"},
                new Peripheral{UId =4343443, Vendor = "Your Enterprise", Status = PeripheralStatus.Offline, CreationDate = DateTimeOffset.Now,GatewayId = "8763-3242-3343-8898"}
            }}} 
        };

        public IQueryable<Gateway> FindAll()
        {
            return new EnumerableQuery<Gateway>(DbContext.Values.ToList());
        }

        public IQueryable<Gateway> FindByCondition(Expression<Func<Gateway, bool>> expression)
        {
            var selectedItems = DbContext.Select(keyValuePair => keyValuePair.Value)
                .Where(expression.Compile());
            return new EnumerableQuery<Gateway>(selectedItems);
        }

        public void Create(Gateway entity)
        {
            var guid = Guid.NewGuid().ToString();
            DbContext.Add(guid, entity);
        }

        public void Update(Gateway entity)
        {
            DbContext[entity.SerialNumber] = entity;
        }

        public void Delete(Gateway entity)
        {
            DbContext.Remove(entity.SerialNumber);
        }

        public async Task<IEnumerable<Gateway>> GetAllGateways()
        {
            return await Task.FromResult(FindAll().OrderBy(gateway => gateway.Name));
        }

        public async Task<Gateway> GetGatewayById(string serialNumber)
            => await Task.FromResult(FindByCondition(gateway => gateway.SerialNumber == serialNumber).FirstOrDefault());
    }
}
