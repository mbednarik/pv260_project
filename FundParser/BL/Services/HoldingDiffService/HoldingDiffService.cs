using AutoMapper;
using DAL.Models;
using DAL.UnitOfWork.Interface;
using Microsoft.EntityFrameworkCore;

namespace BL.Services.HoldingDiffService
{
    public class HoldingDiffService : IHoldingDiffService
    {
        private readonly IUoWHoldingDiff uoWHoldingDiff;
        private readonly IUoWHolding uoWHolding;
        private readonly IMapper mapper;

        public HoldingDiffService(IUoWHoldingDiff uoWHoldingDiff, IUoWHolding uoWHolding,
            IMapper mapper)
        {
            this.uoWHoldingDiff = uoWHoldingDiff;
            this.uoWHolding = uoWHolding;
            this.mapper = mapper;
        }

        private static HoldingDiff GetHoldingDiff(Holding? oldHolding, Holding? newHolding)
        {
            if (oldHolding == null && newHolding == null)
            {
                throw new ArgumentException("Both holdings are null");
            }
            
            if (oldHolding == null)
            {
                ArgumentNullException.ThrowIfNull(newHolding, nameof(newHolding));

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
            
            if (newHolding == null)
            {
                ArgumentNullException.ThrowIfNull(oldHolding, nameof(oldHolding));

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
            
            var holdingDiff = new HoldingDiff
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

            return holdingDiff;
        }

        private static IEnumerable<HoldingDiff> CompareHoldings(IEnumerable<Holding> oldHoldings, IEnumerable<Holding> newHoldings)
        {
            var oldHoldingsArray = oldHoldings as Holding[] ?? oldHoldings.ToArray();
            var newHoldingsArray = newHoldings as Holding[] ?? newHoldings.ToArray();
            
            var oldHoldingDict = oldHoldingsArray.ToDictionary(h => h.CompanyId);
            var newHoldingDict = newHoldingsArray.ToDictionary(h => h.CompanyId);

            var holdingDiffs = oldHoldingsArray
                .Select(h => GetHoldingDiff(h, newHoldingDict.GetValueOrDefault(h.CompanyId)))
                .Concat(newHoldingsArray
                    .Where(h => !oldHoldingDict.ContainsKey(h.CompanyId))
                    .Select(h => GetHoldingDiff(null, h)));

            return holdingDiffs;
        }

        public IEnumerable<HoldingDiff> CalculateHoldingDiffs(DateTime oldHoldingsDate, DateTime newHoldingsDate)
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

            return holdingDiffs;
        }
    }
}
