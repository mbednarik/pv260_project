using BL.DTOs;
using DAL.Models;

namespace BL.Services.HoldingDiffService
{
    public interface IHoldingDiffService
    {
        IEnumerable<HoldingDiff> CalculateHoldingDiffs(DateTime oldHoldingsDate, DateTime newHoldingsDate);
    }
}