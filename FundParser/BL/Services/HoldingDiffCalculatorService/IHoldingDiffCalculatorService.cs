using FundParser.DAL.Models;

namespace FundParser.BL.Services.HoldingDiffCalculatorService;

public interface IHoldingDiffCalculatorService
{
    public List<HoldingDiff> CalculateHoldingDiffs(IEnumerable<Holding> oldHoldings, IEnumerable<Holding> newHoldings);
}