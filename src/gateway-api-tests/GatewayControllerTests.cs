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
            this.repoWrapper = new FakeRepositoryWrapper();
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

       
    }
}
