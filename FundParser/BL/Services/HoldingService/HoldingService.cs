﻿using AutoMapper;
using BL.DTOs;
using DAL.Models;
using DAL.UnitOfWork.Interface;

namespace BL.Services.HoldingService
{
    public class HoldingService : IHoldingService
    {
        private readonly IUoWHolding _uow;
        private readonly IMapper _mapper;

        public HoldingService(IUoWHolding uow,
            IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<IEnumerable<HoldingDTO>> GetHoldings()
        {
            var holdings = await _uow.HoldingRepository.GetAll();
            return _mapper.Map<IEnumerable<HoldingDTO>>(holdings.ToList());
        }

        public async Task<HoldingDTO?> AddHolding(AddHoldingDTO holding)
        {
            try
            {
                var newHolding = _uow.HoldingRepository.Insert(new Holding
                {
                    FundId = holding.FundId,
                    CompanyId = holding.CompanyId,
                    MarketValue = holding.MarketValue,
                    Date = holding.Date,
                    Weight = holding.Weight,
                    Shares = holding.Shares,
                });
                await _uow.CommitAsync();
                return _mapper.Map<HoldingDTO>(newHolding);
            }
            catch (Exception e)
            {
                Console.WriteLine("AddHolding error: {0}", e.Message);
                return null;
            }
        }
    }
}