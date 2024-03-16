using AutoMapper;

using BL.DTOs;

using DAL.Models;
using DAL.UnitOfWork.Interface;

using Microsoft.EntityFrameworkCore;

namespace BL.Services.HoldingService
{
    public class HoldingService : IHoldingService
    {
        private readonly IUoWHolding _uowHolding;
        private readonly IUoWFund _uowFund;
        private readonly IUoWCompany _uowWCompany;
        private readonly IMapper _mapper;

        public HoldingService(
            IUoWHolding uowHolding,
            IUoWFund uowFund,
            IUoWCompany uowWCompany,
            IMapper mapper)
        {
            _uowHolding = uowHolding;
            _uowFund = uowFund;
            _uowWCompany = uowWCompany;
            _mapper = mapper;

        }

        public async Task<IEnumerable<HoldingDTO>> GetHoldings()
        {
            var holdings = await _uowHolding.HoldingRepository.GetAll();
            return _mapper.Map<IEnumerable<HoldingDTO>>(holdings.ToList());
        }

        public async Task<HoldingDTO> AddHolding(AddHoldingDTO holding)
        {
            var existingFund = await _uowFund.FundRepository
                .GetQueryable()
                .FirstOrDefaultAsync(f => f.Name == holding.Fund.Name);
            var existingCompany = await _uowWCompany.CompanyRepository
                .GetQueryable()
                .FirstOrDefaultAsync(f => f.Cusip == holding.Company.Cusip);

            var newHolding = _uowHolding.HoldingRepository.Insert(new Holding
            {
                Fund = existingFund ?? _mapper.Map<Fund>(holding.Fund),
                Company = existingCompany ?? _mapper.Map<Company>(holding.Company),
                MarketValue = holding.MarketValue,
                Date = holding.Date,
                Weight = holding.Weight,
                Shares = holding.Shares,
            });

            return _mapper.Map<HoldingDTO>(newHolding);
        }
    }
}