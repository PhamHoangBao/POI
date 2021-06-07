using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using POI.repository.Entities;
using POI.repository.ViewModels;


namespace POI.repository.AutoMapper
{
    public static class UserModule
    {
        public static void ConfigureUserModule(this IMapperConfigurationExpression mc)
        {

            mc.CreateMap<User, CreateUserViewModel>();
            mc.CreateMap<CreateUserViewModel, User>()
                .ForMember(des => des.Status, options => options.MapFrom(src => 1));
            mc.CreateMap<User, UpdateUserViewModel>();
            mc.CreateMap<UpdateUserViewModel, User>()
                .ForMember(des => des.Status, options => options.MapFrom(src => 1));
        }
    }
}
