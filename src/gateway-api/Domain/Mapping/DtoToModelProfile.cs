﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Gateways.Domain.Resources;

namespace Gateways.Domain.Mapping
{
    public class DtoToModelProfile:Profile
    {
        public DtoToModelProfile()
        {
            CreateMap<PeripheralDetailedDto, Peripheral>();
            CreateMap<PeripheralCreationDto, Peripheral>();
            CreateMap<GatewayDetailedDto, Gateway>();
            CreateMap<GatewayCreateDto, Gateway>();
            CreateMap<GatewayUpdateDto, Gateway>();
        }
    }
}
