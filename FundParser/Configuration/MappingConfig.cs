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
            config.CreateMap<Fund, FundDTO>().ReverseMap();
            config.CreateMap<Fund, AddFundDTO>().ReverseMap();
            config.CreateMap<Company, CompanyDTO>().ReverseMap();
            config.CreateMap<Company, AddCompanyDTO>().ReverseMap();
        }
    }
}
