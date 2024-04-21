using FundParser.DAL.Models;

namespace FundParser.BL.Utils.HoldingDiffCalculator;

public static class HoldingDiffCalculator
{
    public static List<HoldingDiff> CalculateHoldingDiffs(IEnumerable<Holding> oldHoldings, IEnumerable<Holding> newHoldings)
    {
        var oldHoldingsList = oldHoldings.ToList();
        var newHoldingsList = newHoldings.ToList();

        var holdingDiffs = CompareHoldings(oldHoldingsList, newHoldingsList);

        return holdingDiffs.Select<HoldingDiff, HoldingDiff>(hd =>
        {
            hd.OldHoldingDate = oldHoldingsList.FirstOrDefault()?.Date;
            hd.NewHoldingDate = newHoldingsList.FirstOrDefault()?.Date;

            return hd;
        }).ToList();
    }
        
    private static IEnumerable<HoldingDiff> CompareHoldings(IReadOnlyCollection<Holding> oldHoldings, IReadOnlyCollection<Holding> newHoldings)
        {
            var oldHoldingDict = oldHoldings.ToDictionary(h => h.CompanyId);
            var newHoldingDict = newHoldings.ToDictionary(h => h.CompanyId);

            var holdingDiffs = oldHoldings
                .Select(h =>
                {
                    var newHolding = newHoldingDict.GetValueOrDefault(h.CompanyId);

                    return newHolding == null ? GetOldHoldingDiff(h) : GetHoldingDiff(h, newHolding);
                })
                .Concat(newHoldings
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