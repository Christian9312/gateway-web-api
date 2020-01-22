using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Gateways.Domain.Resources;

namespace Gateways.Domain.Mapping
{
    public class ModelToDtoProfile:Profile
    {
        public ModelToDtoProfile()
        {
            CreateMap<Peripheral,PeripheralDetailedDto>();
            CreateMap<Peripheral,PeripheralSimpleDto>();
            CreateMap<Gateway, GatewayCreateDto>();
            CreateMap<Gateway, GatewayDetailedDto>();
            CreateMap<Gateway, GatewaySimpleDto>();
        }
    }
}
