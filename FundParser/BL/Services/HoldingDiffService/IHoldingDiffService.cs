using FundParser.BL.DTOs;

namespace FundParser.BL.Services.HoldingDiffService
{
    public interface IHoldingDiffService
    {
        Task<IEnumerable<HoldingDiffDTO>> GetHoldingDiffs(int fundId, DateTime oldHoldingDate, DateTime newHoldingDate, CancellationToken cancellationToken = default);
    }
}