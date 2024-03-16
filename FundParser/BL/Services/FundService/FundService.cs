using AutoMapper;
using BL.DTOs;
using DAL.Models;
using DAL.UnitOfWork.Interface;
using Microsoft.EntityFrameworkCore;

namespace BL.Services.FundService;

public class FundService : IFundService
{
    private readonly IUoWFund _uow;
    private readonly IMapper _mapper;

    public FundService(IUoWFund uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<FundDTO?> AddFundIfNotExists(AddFundDTO fund)
    {
        try
        {
            var existingFund = await _uow.FundRepository.GetQueryable().FirstOrDefaultAsync(f => f.Name == fund.Name);
            if (existingFund != null)
            {
                return _mapper.Map<FundDTO>(existingFund);
            }

            var newFund = _uow.FundRepository.Insert(new Fund
            {
                Name = fund.Name,
            });
            await _uow.CommitAsync();
            return _mapper.Map<FundDTO>(newFund);
        }
        catch (Exception e)
        {
            Console.WriteLine("AddFundIfNotExists error: {0}", e.Message);
            return null;
        }
    }
}