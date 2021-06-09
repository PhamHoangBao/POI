using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using POI.repository.Entities;
using POI.repository.Enums;
using POI.repository.ViewModels;


namespace POI.repository.AutoMapper
{
    public static class DestinationModule
    {
        public static void ConfigureDestinationModule(this IMapperConfigurationExpression mc)
        {

            mc.CreateMap<Destination, CreateDestinationViewModel>();
            mc.CreateMap<CreateDestinationViewModel, Destination>()
                .ForMember(des => des.Status, options => options.MapFrom(src => DestinationEnum.Available));
            mc.CreateMap<Destination, UpdateDestinationViewModel>();
            mc.CreateMap<UpdateDestinationViewModel, Destination>()
                .ForMember(des => des.Status, options => options.MapFrom(src => DestinationEnum.Available));
        }
    }
}
