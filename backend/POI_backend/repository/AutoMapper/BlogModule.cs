using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using POI.repository.Entities;
using POI.repository.Enums;
using POI.repository.ViewModels;

namespace POI.repository.AutoMapper
{
    public static class BlogModule
    {
        public static void ConfigureBlogModule(this IMapperConfigurationExpression mc)
        {
            mc.CreateMap<CreateBlogViewModel, Blog>()
                .ForMember(des => des.Status, options => options.MapFrom(src => BlogEnum.Pending))
                .ForMember(des => des.NegVotes, options => options.MapFrom(src => 0))
                .ForMember(des => des.PosVotes, options => options.MapFrom(src => 0));
            mc.CreateMap<UpdateBlogViewModel, Blog>()
                .ForMember(des => des.Status, options => options.MapFrom(src => BlogEnum.Pending))
                .ForMember(des => des.NegVotes, options => options.MapFrom(src => 0))
                .ForMember(des => des.PosVotes, options => options.MapFrom(src => 0));
            mc.CreateMap<Blog, ResponseBlogViewModel>()
                .ForMember(des => des.TripName, options => options.MapFrom(src => src.Trip.TripName));
        }
    }
}
