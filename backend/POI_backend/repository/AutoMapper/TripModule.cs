using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using POI.repository.Entities;
using POI.repository.Enums;
using POI.repository.ViewModels;

namespace POI.repository.AutoMapper
{
    public static class TripModule
    {
        public static void ConfigureTripModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<Trip, CreateTripViewModel>();
            mc.CreateMap<CreateTripViewModel, Trip>()
                .ForMember(des => des.Status, options => options.MapFrom(src => TripEnum.ONGOING));

            mc.CreateMap<Trip, UpdateTripViewModel>();
            mc.CreateMap<UpdateTripViewModel, Trip>()
                .ForMember(des => des.Status, options => options.MapFrom(src => TripEnum.ONGOING));
        }
    }
}
