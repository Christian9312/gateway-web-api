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
        private IRepositoryWrapper repoWrapper;
        private IMapper mapper;


        private GatewayController controller;

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
        public async Task SuccessGetAllGateways()
        {
            var result = await controller.GetGateways();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            var items = Assert.IsAssignableFrom<IEnumerable<GatewayDetailedDto>>(okResult.Value);

            Assert.Equal(4, items.Count());
        }

        [Fact]
        public async Task NotFoundGateways()
        {
            var testSerialNumber = "7879-4667-4233-2310";

            var result = await controller.GetGatewayById(testSerialNumber);

           Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetExistingGateway()
        {
            var testSerialNumber = "6521-1434-3451-4531";

            var result = await controller.GetGatewayById(testSerialNumber);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            var item = Assert.IsAssignableFrom<GatewayDetailedDto>(okResult.Value);

            Assert.Equal(item.SerialNumber, testSerialNumber);
        }

        [Fact]
        public async Task CreateValidResponse()
        {
            var correctGateway = new GatewayCreateDto{SerialNumber = "5454-8768-3423-3241", Name = "Network 1",Address = "192.168.1.1"};
            var result = await controller.CreateGateway(correctGateway);

            Assert.IsType<CreatedAtRouteResult>(result);
        }

        [Fact]
        public async Task CreateValidGateway()
        {
            var correctGateway = new GatewayCreateDto{SerialNumber = "5454-8768-3423-3241", Name = "Network 1",Address = "192.168.1.1"};
            await controller.CreateGateway(correctGateway);
            
            var getResult = await controller.GetGateways();

            var okResult = getResult.Result as OkObjectResult;

            var items = okResult?.Value as IEnumerable<GatewayDetailedDto>;

            Assert.Equal(5, items?.Count());

        }

        [Fact]
        public async Task NotCreateInvalidGateway()
        {
            var missingSerialGateway = new GatewayCreateDto
                {
                    Name = "Network 1", 
                    Address = "192.168.1.65"
                };

            controller.ModelState.AddModelError("Serial number", "Required");
            var result = await controller.CreateGateway(missingSerialGateway);

            Assert.IsType<BadRequestObjectResult>(result);
        }

       
    }
}
