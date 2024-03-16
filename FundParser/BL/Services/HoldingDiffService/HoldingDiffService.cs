using BL.DTOs;

using DAL.Models;
using DAL.UnitOfWork.Interface;

using Microsoft.EntityFrameworkCore;

namespace BL.Services.HoldingDiffService
{
    public class HoldingDiffService : IHoldingDiffService
    {
        private readonly IUoWHoldingDiff uoWHoldingDiff;
        private readonly IUoWHolding uoWHolding;

        public HoldingDiffService(IUoWHoldingDiff uoWHoldingDiff, IUoWHolding uoWHolding)
        {
            this.uoWHoldingDiff = uoWHoldingDiff;
            this.uoWHolding = uoWHolding;
        }

        public async Task<IEnumerable<HoldingDiffDTO>> GetHoldingDiffs(int fundId, DateTime oldHoldingDate, DateTime newHoldingDate, CancellationToken cancellationToken = default)
        {
            return await uoWHoldingDiff.HoldingDiffRepository
                .GetQueryable()
                .Where(hd => hd.FundId == fundId)
                .Where(hd =>
                    (hd.OldHolding == null && hd.NewHolding != null && hd.NewHolding.Date == newHoldingDate) ||
                    (hd.NewHolding == null && hd.OldHolding != null && hd.OldHolding.Date == oldHoldingDate) ||
                    (hd.OldHolding != null && hd.NewHolding != null && hd.OldHolding.Date == oldHoldingDate && hd.NewHolding.Date == newHoldingDate))
                .Select(hd => new HoldingDiffDTO
                {
                    Id = hd.Id,
                    FundName = hd.Fund.Name,
                    CompanyName = hd.Company.Name,
                    Ticker = hd.Company.Ticker,
                    OldShares = hd.OldShares,
                    SharesChange = hd.SharesChange,
                    OldWeight = hd.OldWeight,
                    WeightChange = hd.WeightChange
                })
                .ToListAsync(cancellationToken);
        }

        public async Task CalculateAndStoreHoldingDiffs(DateTime oldHoldingsDate, DateTime newHoldingsDate, CancellationToken cancellationToken = default)
        {
            var oldHoldings = uoWHolding.HoldingRepository.GetQueryable()
                .Where(h => h.Date == oldHoldingsDate);
            var newHoldings = uoWHolding.HoldingRepository.GetQueryable()
                .Where(h => h.Date == newHoldingsDate);

            var holdingDiffs = CompareHoldings(oldHoldings, newHoldings).ToList();

            foreach (var holdingDiff in holdingDiffs)
            {
                uoWHoldingDiff.HoldingDiffRepository.Insert(holdingDiff);
            }

            await uoWHoldingDiff.CommitAsync(cancellationToken);
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
    }
}
