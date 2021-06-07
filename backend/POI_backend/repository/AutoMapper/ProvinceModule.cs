using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using POI.repository.Entities;
using POI.repository.ViewModels;

namespace POI.repository.AutoMapper
{
    public static class ProvinceModule
    {
        public static void ConfigureProvinceModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<Province, CreateProvinceViewModel>();
            mc.CreateMap<CreateProvinceViewModel, Province>()
                .ForMember(des => des.Status, options => options.MapFrom(src => 1));
            mc.CreateMap<Province, UpdateProvinceViewModel>();
            mc.CreateMap<UpdateProvinceViewModel, Province>()
                .ForMember(des => des.Status, options => options.MapFrom(src => 1));
        }
    }
}
