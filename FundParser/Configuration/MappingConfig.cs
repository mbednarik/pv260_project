using AutoMapper;
using BL.DTOs;
using DAL.Models;

namespace Configuration
{
    public class MappingConfig
    {
        public static void ConfigureMapping(IMapperConfigurationExpression config)
        {
            config.CreateMap<Holding, HoldingDTO>().ReverseMap();
        }
    }
}
