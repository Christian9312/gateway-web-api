using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Gateways.Domain;
using Gateways.Domain.Contracts;
using Gateways.Domain.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gateways.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeripheralController : ControllerBase
    {
        private IRepositoryWrapper repoWrapper;
        private IMapper mapper;

        public PeripheralController(IRepositoryWrapper repoWrapper, IMapper mapper)
        {
            this.repoWrapper = repoWrapper;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PeripheralDetailedDto>>> GetPeripherals()
        {
            var peripherals = await repoWrapper.Peripheral.GetAllPeripherals();
            var mappedPeripheral = mapper.Map<IEnumerable<PeripheralDetailedDto>>(peripherals);
            return Ok(mappedPeripheral);
        }

        [HttpGet("{id}",Name ="PeripheralById")]
        public async Task<ActionResult<PeripheralDetailedDto>> GetPeripheralById(uint id)
        {
            var peripheral = await repoWrapper.Peripheral.GetPeripheralById(id);
            if (peripheral == null)
                return NotFound();
            var mappedPeripheral = mapper.Map<PeripheralDetailedDto>(peripheral);
            return Ok(mappedPeripheral);
        }

        [HttpPost(Name ="PeripheralCreation")]
        public async Task<IActionResult> CreatePeripheral([FromBody] PeripheralDto peripheralDto)
        {
            if (peripheralDto == null)
            {
                return BadRequest("Peripheral object is null");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid model object");
            }

            var peripheralEntity = mapper.Map<PeripheralDto,Peripheral>(peripheralDto);

            var gateway = await repoWrapper.Gateway.GetGatewayById(peripheralEntity.GatewayId);
            if (gateway == null)
                return BadRequest("Invalid reference to gateway");
            if (gateway.AssociatedPeripherals.Count >= 10)
                return BadRequest("Gateway already has the maximum of admitted peripherals");

            repoWrapper.Peripheral.Create(peripheralEntity);
            var result = await repoWrapper.SaveAsync();

            if (!result.Success)
                return BadRequest(result.Message);

            var peripheral = mapper.Map<Peripheral, PeripheralDetailedDto>(peripheralEntity);

            return CreatedAtRoute("PeripheralById",new {id = peripheralEntity.UId},peripheral);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdatePeripheral(uint id, [FromBody] PeripheralDto peripheralDto)
        {
            if (peripheralDto == null)
            {
                return BadRequest("Peripheral object is null");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid model object");
            }

            var peripheralEntity = await repoWrapper.Peripheral.GetPeripheralById(id);

            if (peripheralEntity == null)
                return NotFound();

            var gateway = await repoWrapper.Gateway.GetGatewayById(peripheralEntity.GatewayId);
            if (gateway == null)
                return BadRequest("Invalid reference to gateway");

            mapper.Map(peripheralDto, peripheralEntity);

            repoWrapper.Peripheral.Update(peripheralEntity);
            var response = await repoWrapper.SaveAsync();
            if (!response.Success)
                return BadRequest(response.Message);

            var peripheral = mapper.Map<Peripheral, PeripheralDetailedDto>(peripheralEntity);

            return Ok(peripheral);

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePeripheral(uint id)
        {
            var peripheralEntity = await repoWrapper.Peripheral.GetPeripheralById(id);
            if (peripheralEntity == null)
                return NotFound("Peripheral not found");

            repoWrapper.Peripheral.Delete(peripheralEntity);
            var response = await repoWrapper.SaveAsync();
            if (!response.Success)
                return BadRequest(response.Message);

            var peripheral = mapper.Map<Peripheral, PeripheralDetailedDto>(peripheralEntity);

            return Ok(peripheral);
        }
    }
}