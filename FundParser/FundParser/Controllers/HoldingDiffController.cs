using BL.DTOs;
using BL.Services.HoldingDiffService;

using Microsoft.AspNetCore.Mvc;

namespace FundParser.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HoldingDiffController : ControllerBase
    {
        private readonly IHoldingDiffService holdingDiffService;

        public HoldingDiffController(IHoldingDiffService holdingDiffService)
        {
            this.holdingDiffService = holdingDiffService;
        }

        [HttpGet(Name = nameof(GetHoldingDiffs))]
        public Task<IEnumerable<HoldingDiffDTO>> GetHoldingDiffs(int fundId, DateTime oldHoldingDate, DateTime newHoldingDate, CancellationToken cancellationToken)
        {
            return holdingDiffService.GetHoldingDiffs(fundId, oldHoldingDate, newHoldingDate, cancellationToken);
        }
    }
}
