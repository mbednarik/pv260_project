using FundParser.DAL.Models;

namespace FundParser.BL.Services.HoldingDiffCalculatorService;

public class HoldingDiffCalculatorService : IHoldingDiffCalculatorService
{
    public IEnumerable<HoldingDiff> CalculateHoldingDiffs(IEnumerable<Holding> oldHoldings,
        IEnumerable<Holding> newHoldings)
    {
        var oldHoldingsList = oldHoldings.ToList();
        var newHoldingsList = newHoldings.ToList();

        return CompareHoldings(oldHoldingsList, newHoldingsList);
    }

    private static IEnumerable<HoldingDiff> CompareHoldings(IReadOnlyCollection<Holding> oldHoldings,
        IReadOnlyCollection<Holding> newHoldings)
    {
        var oldHoldingDict = oldHoldings.ToDictionary(h => h.CompanyId);
        var newHoldingDict = newHoldings.ToDictionary(h => h.CompanyId);

        var holdingDiffs = oldHoldings
            .Select(h =>
            {
                var newHolding = newHoldingDict.GetValueOrDefault(h.CompanyId);

                return newHolding is null ? GetOldHoldingDiff(h) : GetHoldingDiff(h, newHolding);
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
            Fund = newHolding.Fund,
            CompanyId = newHolding.CompanyId,
            Company = newHolding.Company,
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
            Fund = oldHolding.Fund,
            CompanyId = oldHolding.CompanyId,
            Company = oldHolding.Company,
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
            Fund = oldHolding.Fund,
            CompanyId = oldHolding.CompanyId,
            Company = oldHolding.Company,
            OldHoldingId = oldHolding.Id,
            NewHoldingId = newHolding.Id,
            OldShares = oldHolding.Shares,
            SharesChange = newHolding.Shares - oldHolding.Shares,
            OldWeight = oldHolding.Weight,
            WeightChange = newHolding.Weight - oldHolding.Weight
        };
    }
}