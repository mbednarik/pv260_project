using AutoMapper;

using FundParser.BL.DTOs;
using FundParser.DAL.Models;
using FundParser.DAL.UnitOfWork;

using Microsoft.EntityFrameworkCore;

namespace FundParser.BL.Services.HoldingService
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

        public async Task<IEnumerable<HoldingDTO>> GetHoldings(CancellationToken cancellationToken = default)
        {
            var holdings = await _unitOfWork.HoldingRepository.GetAll(cancellationToken);
            return _mapper.Map<IEnumerable<HoldingDTO>>(holdings.ToList());
        }

        public async Task<HoldingDTO> AddHolding(AddHoldingDTO holding, CancellationToken cancellationToken = default)
        {
            var existingFund = await _unitOfWork.FundRepository
                .GetQueryable()
                .FirstOrDefaultAsync(f => f.Name == holding.Fund.Name, cancellationToken);
            var existingCompany = await _unitOfWork.CompanyRepository
                .GetQueryable()
                .FirstOrDefaultAsync(f => f.Cusip == holding.Company.Cusip, cancellationToken);

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