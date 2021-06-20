using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using POI.repository.Entities;
using POI.repository.Enums;
using POI.repository.ViewModels;


namespace POI.repository.AutoMapper
{
    public static class UserModule
    {
        public static void ConfigureUserModule(this IMapperConfigurationExpression mc)
        {

            mc.CreateMap<User, CreateUserViewModel>();
            mc.CreateMap<CreateUserViewModel, User>()
                .ForMember(des => des.Status, options => options.MapFrom(src => UserEnum.Active));
            mc.CreateMap<User, UpdateUserViewModel>();
            mc.CreateMap<UpdateUserViewModel, User>()
                .ForMember(des => des.Status, options => options.MapFrom(src => UserEnum.Active));
            mc.CreateMap<User, AuthenticatedUserViewModel>()
                .ForMember(des => des.RoleName, options => options.MapFrom(src => src.Role.RoleName));
            mc.CreateMap<AuthenticatedUserViewModel, User>()
                .ForMember(des => des.Status, options => options.MapFrom(src => UserEnum.Active));
        }
    }
}
