using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using gateway_api_tests;
using Gateways.Controllers;
using Gateways.Domain;
using Gateways.Domain.Contracts;
using Gateways.Domain.Extensions;
using Gateways.Domain.Mapping;
using Gateways.Domain.Resources;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace peripheral_api_tests
{
    public class PeripheralControllerTests
    {
        private readonly IRepositoryWrapper repoWrapper;
        private readonly IMapper mapper;


        private readonly PeripheralController controller;

        public PeripheralControllerTests()
        {
            var config = new MapperConfiguration(opt =>
            {
                opt.AddProfile(typeof(DtoToModelProfile));
                opt.AddProfile(typeof(ModelToDtoProfile));
            });
            this.mapper = config.CreateMapper();
            this.repoWrapper = new FakeRepositoryWrapper();
            this.controller = new PeripheralController(repoWrapper, mapper);
        }

        [Fact]
        public async Task GetAllPeripherals_Success()
        {
            var result = await controller.GetPeripherals();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            var items = Assert.IsAssignableFrom<IEnumerable<PeripheralDetailedDto>>(okResult.Value);

            Assert.Equal(12, items.Count());
        }

        [Fact]
        public async Task GetAllPeripherals_ReturnsNotFound()
        {
            const int id = 75453453;

            var result = await controller.GetPeripheralById(id);

           Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetExistingPeripheral_ReturnsCorrectItem()
        {
            const uint id = 3231232;

            var result = await controller.GetPeripheralById(id);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            var item = Assert.IsAssignableFrom<PeripheralDetailedDto>(okResult.Value);

            Assert.Equal(item.UId, id);
        }

        [Fact]
        public async Task CreatePeripheralMissingGatewayReference_ResultBadRequest()
        {
            var correctPeripheral = new PeripheralDto { Vendor = "Enterprise INC", CreationDate = DateTimeOffset.Now, Status = PeripheralStatus.Online};
            var result = await controller.CreatePeripheral(correctPeripheral);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task CreatePeripheral_ResultOK()
        {
            var correctPeripheral = new PeripheralDto { Vendor = "Enterprise INC", CreationDate = DateTimeOffset.Now, Status = PeripheralStatus.Online, GatewayId = "4451-9834-7885-3446"};
            var result = await controller.CreatePeripheral(correctPeripheral);

            Assert.IsType<CreatedAtRouteResult>(result);
        }

        [Fact]
        public async Task CreateValidPeripheral_AddItem()
        {
            var correctPeripheral = new PeripheralDto { Vendor = "Enterprise INC", CreationDate = DateTimeOffset.Now, Status = PeripheralStatus.Online, GatewayId = "4451-9834-7885-3446"};
            await controller.CreatePeripheral(correctPeripheral);

            var getResult = await controller.GetPeripherals();

            var okResult = getResult.Result as OkObjectResult;

            var items = okResult?.Value as IEnumerable<PeripheralDetailedDto>;

            Assert.Equal(13, items?.Count());

        }

        [Fact]
        public async Task CreatePeripheral_ReturnsBadRequest_RequiredField()
        {
            var missingSerialPeripheral = new PeripheralDto
            {
                Vendor = "Enterprise INC", 
                CreationDate = DateTimeOffset.Now,
                GatewayId = "4451-9834-7885-3446"
            };

            controller.ModelState.AddModelError("Status", "Required");
            var result = await controller.CreatePeripheral(missingSerialPeripheral);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        

        [Fact]
        public async Task CreatePeripheral_ReturnsBadRequest_MaxPeripheralAdmitted()
        {
            var peripheral = new PeripheralDto
            {
                Vendor = "Enterprise INC", 
                CreationDate = DateTimeOffset.Now,
                Status = PeripheralStatus.Online,
                GatewayId = "8763-3242-3343-8898"
            };

            var result = await controller.CreatePeripheral(peripheral);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task UpdatePeripheral__ReturnsOk()
        {
            const uint id = 3231232;
            var inputPeripheral = new PeripheralDto { Vendor = "Modified Enterprise INC", CreationDate = DateTimeOffset.Now, Status = PeripheralStatus.Offline, GatewayId = "4451-9834-7885-3446"};
            var updateResult = await controller.UpdatePeripheral(id, inputPeripheral);

            Assert.IsType<OkObjectResult>(updateResult);
        }

        [Fact]
        public async Task UpdatePeripheral__ReturnsUpdatedItem()
        {
            const uint id = 3231232;
            var inputPeripheral = new PeripheralDto { Vendor = "Modified Enterprise INC", CreationDate = DateTimeOffset.Now, Status = PeripheralStatus.Offline, GatewayId = "4451-9834-7885-3446"};
            await controller.UpdatePeripheral(id, inputPeripheral);

            var getAction = await controller.GetPeripheralById(id);

            var okGetResult = getAction.Result as OkObjectResult;

            var updatedPeripheralDb = okGetResult?.Value as PeripheralDetailedDto;

            Assert.Equal(updatedPeripheralDb?.Status, inputPeripheral.Status.ToDescriptionString());
            Assert.Equal(updatedPeripheralDb?.Vendor, inputPeripheral.Vendor);
        }

        [Fact]
        public async Task DeletePeripheral_ReturnsOk()
        {
            const uint id = 3231232;
            var deletedResult = await controller.DeletePeripheral(id);

            Assert.IsType<OkObjectResult>(deletedResult);
        }

        [Fact]
        public async Task DeletePeripheral_RemoveOneItem()
        {
            const uint id = 3231232;
            await controller.DeletePeripheral(id);

            var getAction = await controller.GetPeripheralById(id);

            Assert.IsType<NotFoundResult>(getAction.Result);

            var getResult = await controller.GetPeripherals();

            var okResult = getResult.Result as OkObjectResult;

            var items = okResult?.Value as IEnumerable<PeripheralDetailedDto>;

            Assert.Equal(11, items?.Count());

        }
    }
}
