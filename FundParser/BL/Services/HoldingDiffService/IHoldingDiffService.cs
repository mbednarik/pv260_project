using BL.DTOs;
using DAL.Models;

namespace BL.Services.HoldingDiffService
{
    public interface IHoldingDiffService
    {
        void CalculateAndStoreHoldingDiffs(DateTime oldHoldingsDate, DateTime newHoldingsDate);
    }
}