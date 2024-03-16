using AutoMapper;
using DAL.Models;
using DAL.UnitOfWork.Interface;

namespace BL.Services.HoldingDiffService
{
    public class HoldingDiffService : IHoldingDiffService
    {
        private readonly IUoWHoldingDiff uoWHoldingDiff;
        private readonly IUoWHolding uoWHolding;

        public HoldingDiffService(IUoWHoldingDiff uoWHoldingDiff, IUoWHolding uoWHolding,
            IMapper mapper)
        {
            this.uoWHoldingDiff = uoWHoldingDiff;
            this.uoWHolding = uoWHolding;
        }
        
        public void CalculateAndStoreHoldingDiffs(DateTime oldHoldingsDate, DateTime newHoldingsDate)
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
    }
}
