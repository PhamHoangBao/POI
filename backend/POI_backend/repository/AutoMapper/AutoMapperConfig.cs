using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using POI.repository.AutoMapper;

namespace POI.repository.AutoMapper
{
    public static class AutoMapperConfig
    {
        public static void ConfigureMapper(this IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(mc => 
            {
                mc.ConfigureUserModule();
                mc.ConfigureHashtagModule();
                mc.ConfigureProvinceModule();
                mc.ConfigureDestinationModule();
                mc.ConfigureDesTypeModule();

                mc.ConfigurePoiModule();
                mc.ConfigurePoiTypeModule();
                mc.ConfigureTripModule();
                mc.ConfigureVisitModule();
                mc.ConfigureVoteModule();
                mc.ConfigureTripDestinationModule();
                mc.ConfigureBlogModule();

                mc.ConfigureCustomModule();
            }
            )
            ;
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}
