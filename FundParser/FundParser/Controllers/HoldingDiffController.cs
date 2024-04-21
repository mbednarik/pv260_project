using FundParser.BL.DTOs;
using FundParser.BL.Services.HoldingDiffService;

using Microsoft.AspNetCore.Mvc;

namespace FundParser.App.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HoldingDiffController : ControllerBase
    {
        private readonly IHoldingDiffService _holdingDiffService;

        public HoldingDiffController(IHoldingDiffService holdingDiffService)
        {
            _holdingDiffService = holdingDiffService;
        }

        [HttpGet(Name = nameof(GetHoldingDiffs))]
        public Task<IEnumerable<HoldingDiffDTO>> GetHoldingDiffs(int fundId, DateTime oldHoldingDate, DateTime newHoldingDate, CancellationToken cancellationToken)
        {
            return _holdingDiffService.GetHoldingDiffs(fundId, oldHoldingDate, newHoldingDate, cancellationToken);
        }
    }
}
