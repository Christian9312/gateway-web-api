using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Gateways.Controllers;
using Gateways.Domain;
using Gateways.Domain.Contracts;
using Gateways.Domain.Mapping;
using Gateways.Domain.Resources;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace gateway_api_tests
{
    public class GatewayControllerTests
    {
        private readonly IRepositoryWrapper repoWrapper;
        private readonly IMapper mapper;


        private readonly GatewayController controller;

        public GatewayControllerTests()
        {
            var config = new MapperConfiguration(opt =>
            {
                opt.AddProfile(typeof(DtoToModelProfile));
                opt.AddProfile(typeof(ModelToDtoProfile));
            });
            this.mapper = config.CreateMapper();
            this.repoWrapper = new MockRepositoryWrapper();
            this.controller = new GatewayController(repoWrapper, mapper);
        }

        [Fact]
        public async Task GetAllGateways_Success()
        {
            var result = await controller.GetGateways();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            var items = Assert.IsAssignableFrom<IEnumerable<GatewayDetailedDto>>(okResult.Value);

            Assert.Equal(4, items.Count());
        }

        [Fact]
        public async Task GetAllGateways_ReturnsNotFound()
        {
            const string testSerialNumber = "7879-4667-4233-2310";

            var result = await controller.GetGatewayById(testSerialNumber);

           Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetExistingGateway_ReturnsCorrectItem()
        {
            const string testSerialNumber = "6521-1434-3451-4531";

            var result = await controller.GetGatewayById(testSerialNumber);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            var item = Assert.IsAssignableFrom<GatewayDetailedDto>(okResult.Value);

            Assert.Equal(item.SerialNumber, testSerialNumber);
        }

        [Fact]
        public async Task CreateGateway_ResultOK()
        {
            var correctGateway = new GatewayDto{Name = "Network 1",Address = "192.168.1.1"};
            var result = await controller.CreateGateway(correctGateway);

            Assert.IsType<CreatedAtRouteResult>(result);
        }

        [Fact]
        public async Task CreateValidGateway_AddItem()
        {
            var correctGateway = new GatewayDto{Name = "Network 1",Address = "192.168.1.1"};
            await controller.CreateGateway(correctGateway);
            
            var getResult = await controller.GetGateways();

            var okResult = getResult.Result as OkObjectResult;

            var items = okResult?.Value as IEnumerable<GatewayDetailedDto>;

            Assert.Equal(5, items?.Count());

        }

        [Fact]
        public async Task CreateGateway_ReturnsBadRequest()
        {
            var missingSerialGateway = new GatewayDto
                {
                    Name = "Network 1", 
                    Address = "192.168.1.665"
                };

            controller.ModelState.AddModelError("Address", "Not valid");
            var result = await controller.CreateGateway(missingSerialGateway);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task UpdateGateway__ReturnsOk()
        {
            const string id = "6521-1434-3451-4531";
            var inputGateway = new GatewayDto{Name = "Network one", Address = "192.168.1.11"};
            var updateResult = await controller.UpdateGateway(id, inputGateway);
            
            Assert.IsType<OkObjectResult>(updateResult);
        }

        [Fact]
        public async Task UpdateGateway__ReturnsUpdatedItem()
        {
            const string id = "6521-1434-3451-4531";
            var inputGateway = new GatewayDto{Name = "Network one", Address = "192.168.1.11"};
            await controller.UpdateGateway(id, inputGateway);

            var getAction = await controller.GetGatewayById(id);

            var okGetResult = getAction.Result as OkObjectResult;

            var updatedGatewayDb = okGetResult?.Value as GatewayDetailedDto;

            Assert.Equal(updatedGatewayDb?.Name, inputGateway.Name);
            Assert.Equal(updatedGatewayDb?.Address, inputGateway.Address);
        }

        [Fact]
        public async Task DeleteGateway_ReturnsOk()
        {
            const string id = "6521-1434-3451-4531";
            var deletedResult = await controller.DeleteGateway(id);
            
            Assert.IsType<OkObjectResult>(deletedResult);
        }

        [Fact]
        public async Task DeleteGateway_RemoveOneItem()
        {
            const string id = "6521-1434-3451-4531";
            await controller.DeleteGateway(id);
            
            var getAction = await controller.GetGatewayById(id);

            Assert.IsType<NotFoundResult>(getAction.Result);

            var getResult = await controller.GetGateways();

            var okResult = getResult.Result as OkObjectResult;

            var items = okResult?.Value as IEnumerable<GatewayDetailedDto>;

            Assert.Equal(3, items?.Count());

        }

        [Fact]
        public async Task DeleteGatewayWithPeripheral_ReturnsBadRequest()
        {
            const string id = "4451-9834-7885-3446";
            
            var deletedResult = await controller.DeleteGateway(id);

            Assert.IsType<BadRequestObjectResult>(deletedResult);

        }
    }
}
