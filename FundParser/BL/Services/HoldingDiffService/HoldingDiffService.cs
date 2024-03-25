using FundParser.BL.DTOs;
using FundParser.DAL.Models;
using FundParser.DAL.UnitOfWork;

using Microsoft.EntityFrameworkCore;

namespace FundParser.BL.Services.HoldingDiffService
{
    public class HoldingDiffService : IHoldingDiffService
    {
        private readonly IUnitOfWork _unitOfWork;

        public HoldingDiffService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
                return await CalculateAndStoreHoldingDiffs(fundId, oldHoldingDate, newHoldingDate, cancellationToken);
            }

            return await _unitOfWork.HoldingDiffRepository.GetQueryable()
                .Where(hd => hd.FundId == fundId)
                .Where(hd =>
                    hd.OldHolding == null && hd.NewHolding != null && hd.NewHolding.Date == newHoldingDate ||
                    hd.NewHolding == null && hd.OldHolding != null && hd.OldHolding.Date == oldHoldingDate ||
                    hd.OldHolding != null && hd.NewHolding != null && hd.OldHolding.Date == oldHoldingDate && hd.NewHolding.Date == newHoldingDate)
                .Select(hd => GetHoldingDiffDTO(hd))
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<HoldingDiffDTO>> CalculateAndStoreHoldingDiffs(int fundId, DateTime oldHoldingsDate, DateTime newHoldingsDate, CancellationToken cancellationToken = default)
        {
            var oldHoldings = _unitOfWork.HoldingRepository.GetQueryable()
                .Where(h => h.FundId == fundId)
                .Where(h => h.Date == oldHoldingsDate);
            var newHoldings = _unitOfWork.HoldingRepository.GetQueryable()
                .Where(h => h.FundId == fundId)
                .Where(h => h.Date == newHoldingsDate);

            var holdingDiffs = CompareHoldings(oldHoldings, newHoldings)
                .Select(hd =>
                {
                    hd.OldHoldingDate = oldHoldingsDate;
                    hd.NewHoldingDate = newHoldingsDate;

                    return hd;
                })
                .ToList();

            foreach (var holdingDiff in holdingDiffs)
            {
                _unitOfWork.HoldingDiffRepository.Insert(holdingDiff);
            }

            await _unitOfWork.CommitAsync(cancellationToken);

            return holdingDiffs.Select(GetHoldingDiffDTO);
        }

        private static IEnumerable<HoldingDiff> CompareHoldings(IEnumerable<Holding> oldHoldings, IEnumerable<Holding> newHoldings)
        {
            var oldHoldingsArray = oldHoldings as Holding[] ?? oldHoldings.ToArray();
            var newHoldingsArray = newHoldings as Holding[] ?? newHoldings.ToArray();

            var oldHoldingDict = oldHoldingsArray.ToDictionary(h => h.CompanyId);
            var newHoldingDict = newHoldingsArray.ToDictionary(h => h.CompanyId);

            var holdingDiffs = oldHoldingsArray
                .Select(h =>
                {
                    var newHolding = newHoldingDict.GetValueOrDefault(h.CompanyId);

                    return newHolding == null ? GetOldHoldingDiff(h) : GetHoldingDiff(h, newHolding);
                })
                .Concat(newHoldingsArray
                    .Where(h => !oldHoldingDict.ContainsKey(h.CompanyId))
                    .Select(GetNewHoldingDiff));

            return holdingDiffs;
        }

        private static HoldingDiff GetNewHoldingDiff(Holding newHolding)
        {
            return new HoldingDiff
            {
                FundId = newHolding.FundId,
                CompanyId = newHolding.CompanyId,
                NewHoldingId = newHolding.Id,
                OldShares = 0,
                SharesChange = newHolding.Shares,
                OldWeight = 0,
                WeightChange = newHolding.Weight
            };
        }

        private static HoldingDiff GetOldHoldingDiff(Holding oldHolding)
        {
            return new HoldingDiff
            {
                FundId = oldHolding.FundId,
                CompanyId = oldHolding.CompanyId,
                OldHoldingId = oldHolding.Id,
                OldShares = oldHolding.Shares,
                SharesChange = -oldHolding.Shares,
                OldWeight = oldHolding.Weight,
                WeightChange = -oldHolding.Weight
            };
        }

        private static HoldingDiff GetHoldingDiff(Holding oldHolding, Holding newHolding)
        {
            return new HoldingDiff
            {
                FundId = oldHolding.FundId,
                CompanyId = oldHolding.CompanyId,
                OldHoldingId = oldHolding.Id,
                NewHoldingId = newHolding.Id,
                OldShares = oldHolding.Shares,
                SharesChange = newHolding.Shares - oldHolding.Shares,
                OldWeight = oldHolding.Weight,
                WeightChange = newHolding.Weight - oldHolding.Weight
            };
        }

        private static HoldingDiffDTO GetHoldingDiffDTO(HoldingDiff hd)
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
