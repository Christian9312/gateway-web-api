using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Gateways.Domain.Extensions;
using Gateways.Domain.Resources;

namespace Gateways.Domain.Mapping
{
    public class ModelToDtoProfile:Profile
    {
        public ModelToDtoProfile()
        {
            CreateMap<Peripheral,PeripheralDetailedDto>().ForMember(src => src.Status,
                opt => opt.MapFrom(src => src.Status.ToDescriptionString()));
            CreateMap<Peripheral,PeripheralSimpleDto>().ForMember(src => src.Status,
                opt => opt.MapFrom(src => src.Status.ToDescriptionString()));
            CreateMap<Gateway, GatewayDto>();
            CreateMap<Gateway, GatewayDetailedDto>();
            CreateMap<Gateway, GatewaySimpleDto>();
        }
    }
}
