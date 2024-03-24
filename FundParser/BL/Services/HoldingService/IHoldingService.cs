using FundParser.BL.DTOs;

namespace FundParser.BL.Services.HoldingService
{
    public interface IHoldingService
    {
        Task<IEnumerable<HoldingDTO>> GetHoldings(CancellationToken cancellationToken = default);

        Task<HoldingDTO> AddHolding(AddHoldingDTO holding, CancellationToken cancellationToken = default);
    }
}
