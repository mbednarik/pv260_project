using AutoMapper;

using BL.DTOs;

using DAL.Models;
using DAL.UnitOfWork;

using Microsoft.EntityFrameworkCore;

namespace BL.Services.HoldingService
{
    public class HoldingService : IHoldingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public HoldingService(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }

        public async Task<IEnumerable<HoldingDTO>> GetHoldings()
        {
            var holdings = await _unitOfWork.HoldingRepository.GetAll();
            return _mapper.Map<IEnumerable<HoldingDTO>>(holdings.ToList());
        }

        public async Task<HoldingDTO> AddHolding(AddHoldingDTO holding)
        {
            var existingFund = await _unitOfWork.FundRepository
                .GetQueryable()
                .FirstOrDefaultAsync(f => f.Name == holding.Fund.Name);
            var existingCompany = await _unitOfWork.CompanyRepository
                .GetQueryable()
                .FirstOrDefaultAsync(f => f.Cusip == holding.Company.Cusip);

            var newHolding = _unitOfWork.HoldingRepository.Insert(new Holding
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