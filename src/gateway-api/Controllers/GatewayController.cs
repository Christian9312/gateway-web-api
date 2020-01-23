using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Gateways.Domain;
using Gateways.Domain.Contracts;
using Gateways.Domain.Resources;
using Microsoft.AspNetCore.Mvc;

namespace Gateways.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GatewayController : ControllerBase
    {
        private IRepositoryWrapper repoWrapper;
        private IMapper mapper;

        public GatewayController(IRepositoryWrapper repoWrapper, IMapper mapper)
        {
            this.repoWrapper = repoWrapper;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GatewayDetailedDto>>> GetGateways()
        {
            var gateways = await repoWrapper.Gateway.GetAllGateways();
            var mappedGateway = mapper.Map<IEnumerable<GatewayDetailedDto>>(gateways);
            return Ok(mappedGateway);
        }

        [HttpGet("{id}",Name ="GatewayById")]
        public async Task<ActionResult<GatewayDetailedDto>> GetGatewayById(string id)
        {
            var gateway = await repoWrapper.Gateway.GetGatewayById(id);
            if (gateway == null)
                return NotFound();
            var mappedGateway = mapper.Map<GatewayDetailedDto>(gateway);
            return Ok(mappedGateway);
        }

        [HttpPost(Name ="GatewayCreation")]
        public async Task<ActionResult> CreateGateway([FromBody] GatewayDto gatewayDto)
        {
            if (gatewayDto == null)
            {
                return BadRequest("Gateway object is null");
            }
            if (!ModelState.IsValid)
            {
                
                return BadRequest("Invalid model object");
            }

            var gatewayEntity = mapper.Map<Gateway>(gatewayDto);
            repoWrapper.Gateway.Create(gatewayEntity);
            var result = await repoWrapper.SaveAsync();

            if (!result.Success)
                return BadRequest(result.Message);

            var gatewayResult = mapper.Map<Gateway, GatewayDetailedDto>(gatewayEntity);

            return CreatedAtRoute("GatewayById",new {id = gatewayEntity.SerialNumber},gatewayResult);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateGateway(string id, [FromBody] GatewayDto gatewayDto)
        {
            if (gatewayDto == null)
            {
                return BadRequest("Gateway object is null");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid model object");
            }

            var gatewayEntity = await repoWrapper.Gateway.GetGatewayById(id);

            if (gatewayEntity == null)
                return NotFound();

            mapper.Map(gatewayDto, gatewayEntity);

            repoWrapper.Gateway.Update(gatewayEntity);
            var response = await repoWrapper.SaveAsync();
            if (!response.Success)
                return BadRequest(response.Message);

            var gatewayResult = mapper.Map<Gateway, GatewayDetailedDto>(gatewayEntity);

            return Ok(gatewayResult);

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteGateway(string id)
        {
            var gatewayEntity = await repoWrapper.Gateway.GetGatewayById(id);
            if (gatewayEntity == null)
                return NotFound();

            var peripheralsInGateway = await repoWrapper.Peripheral.GetPeripheralByGateways(id);
            if (peripheralsInGateway.Any())
                return BadRequest("Cannot delete gateway. It has related peripherals. Delete those peripherals first");

            repoWrapper.Gateway.Delete(gatewayEntity);
            var response = await repoWrapper.SaveAsync();
            if (!response.Success)
                return BadRequest(response.Message);

            var gatewayResult = mapper.Map<Gateway, GatewayDetailedDto>(gatewayEntity);

            return Ok(gatewayResult);
        }
    }
}
