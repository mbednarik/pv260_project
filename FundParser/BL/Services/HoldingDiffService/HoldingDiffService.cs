﻿using FundParser.BL.DTOs;
using FundParser.BL.Services.HoldingDiffCalculatorService;
using FundParser.DAL.Models;
using FundParser.DAL.UnitOfWork;

using Microsoft.EntityFrameworkCore;

namespace FundParser.BL.Services.HoldingDiffService
{
    public class HoldingDiffService : IHoldingDiffService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHoldingDiffCalculatorService _holdingDiffCalculatorService;

        public HoldingDiffService(IUnitOfWork unitOfWork, IHoldingDiffCalculatorService holdingDiffCalculatorServiceService)
        {
            _unitOfWork = unitOfWork;
            _holdingDiffCalculatorService = holdingDiffCalculatorServiceService;
        }
        
        public async Task<IEnumerable<HoldingDiffDTO>> GetHoldingDiffs(int fundId, DateTime oldHoldingDate, DateTime newHoldingDate, CancellationToken cancellationToken = default)
        {
            var areHoldingDiffsCalculated = await _unitOfWork.HoldingDiffRepository.GetQueryable()
                .AnyAsync(hd =>
                    hd.FundId == fundId &&
                    hd.OldHoldingDate == oldHoldingDate &&
                    hd.NewHoldingDate == newHoldingDate,
                    cancellationToken);

            if (!areHoldingDiffsCalculated)
            {
                return await CalculateHoldingDiffs(fundId, oldHoldingDate, newHoldingDate, cancellationToken);
            }

            return await _unitOfWork.HoldingDiffRepository.GetQueryable()
                .Where(hd => hd.FundId == fundId)
                .Where(hd =>
                    hd.OldHolding == null && hd.NewHolding != null && hd.NewHolding.Date == newHoldingDate ||
                    hd.NewHolding == null && hd.OldHolding != null && hd.OldHolding.Date == oldHoldingDate ||
                    hd.OldHolding != null && hd.NewHolding != null && hd.OldHolding.Date == oldHoldingDate && hd.NewHolding.Date == newHoldingDate)
                .Include(hd => hd.Fund)
                .Include(hd => hd.Company)
                .Select(hd => GetHoldingDiffDto(hd))
                .ToListAsync(cancellationToken);
        }

        private async Task<IEnumerable<HoldingDiffDTO>> CalculateHoldingDiffs(int fundId, DateTime oldHoldingsDate, DateTime newHoldingsDate, CancellationToken cancellationToken = default)
        {
            var oldHoldings = _unitOfWork.HoldingRepository.GetQueryable()
                .Where(h => h.FundId == fundId)
                .Where(h => h.Date == oldHoldingsDate);
            var newHoldings = _unitOfWork.HoldingRepository.GetQueryable()
                .Where(h => h.FundId == fundId)
                .Where(h => h.Date == newHoldingsDate);

            var holdingDiffs = _holdingDiffCalculatorService.CalculateHoldingDiffs(oldHoldings, newHoldings).Select(
                hd =>
                {
                    hd.OldHoldingDate = oldHoldingsDate;
                    hd.NewHoldingDate = newHoldingsDate;

                    return hd;
                }
            );
            
            var holdingDiffsList = holdingDiffs.ToList();

            foreach (var holdingDiff in holdingDiffsList)
            {
                await _unitOfWork.HoldingDiffRepository.Insert(holdingDiff, cancellationToken);
            }

            await _unitOfWork.CommitAsync(cancellationToken);

            return holdingDiffsList.Select(GetHoldingDiffDto);
        }

        private static HoldingDiffDTO GetHoldingDiffDto(HoldingDiff hd)
        {
            return new HoldingDiffDTO
            {
                Id = hd.Id,
                FundName = hd.Fund.Name,
                CompanyName = hd.Company.Name,
                Ticker = hd.Company.Ticker,
                OldShares = hd.OldShares,
                SharesChange = hd.SharesChange,
                OldWeight = hd.OldWeight,
                WeightChange = hd.WeightChange
            };
        }
    }
}
