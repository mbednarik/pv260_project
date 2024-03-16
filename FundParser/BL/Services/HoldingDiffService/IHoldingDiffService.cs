using BL.DTOs;

namespace BL.Services.HoldingDiffService
{
    public interface IHoldingDiffService
    {
        Task<IEnumerable<HoldingDiffDTO>> GetHoldingDiffs(int fundId, DateTime oldHoldingDate, DateTime newHoldingDate, CancellationToken cancellationToken = default);

        Task CalculateAndStoreHoldingDiffs(DateTime oldHoldingsDate, DateTime newHoldingsDate, CancellationToken cancellationToken = default);
    }
}