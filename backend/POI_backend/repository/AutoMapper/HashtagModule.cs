using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using POI.repository.Entities;
using POI.repository.Enums;
using POI.repository.ViewModels;

namespace POI.repository.AutoMapper
{
    public static class HashtagModule
    {
        public static void ConfigureHashtagModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<Hashtag, CreateHashtagViewModel>();
            mc.CreateMap<CreateHashtagViewModel, Hashtag>()
                .ForMember(des => des.Status, options => options.MapFrom(src => HashtagEnum.Available));
            mc.CreateMap<Hashtag, UpdateHashtagViewModel>();
            mc.CreateMap<UpdateHashtagViewModel, Hashtag>()
                .ForMember(des => des.Status, options => options.MapFrom(src => HashtagEnum.Available));

        }
    }
}
