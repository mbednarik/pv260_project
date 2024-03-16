using BL.DTOs;

namespace BL.Services.FundService;

public interface IFundService
{
    Task<FundDTO?> AddFundIfNotExists(AddFundDTO fund);
}