using BL.DTOs;
using BL.Services.HoldingDiffService;

using Microsoft.AspNetCore.Mvc;

namespace FundParser.Controllers
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
