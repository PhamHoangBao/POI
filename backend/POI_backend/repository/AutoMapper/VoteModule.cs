using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using POI.repository.Entities;
using POI.repository.Enums;
using POI.repository.ViewModels;

namespace POI.repository.AutoMapper
{
    public static class VoteModule
    {
        public static void ConfigureVoteModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<Vote, CreateVoteViewModel>();
            mc.CreateMap<CreateVoteViewModel, Vote>()
                .ForMember(des => des.Status, options => options.MapFrom(src => VoteEnum.Available));
            mc.CreateMap<Vote, UpdateVoteViewModel>();
            mc.CreateMap<UpdateVoteViewModel, Vote>()
                .ForMember(des => des.Status, options => options.MapFrom(src => VoteEnum.Available));
        }
    }
}
