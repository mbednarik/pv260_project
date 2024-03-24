using BL.Services.HoldingService;

using Microsoft.AspNetCore.Mvc;

namespace FundParser.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HoldingController : ControllerBase
    {
        private readonly IHoldingService _holdingService;

        public HoldingController(IHoldingService holdingService)
        {
            _holdingService = holdingService;
        }

        [HttpGet(Name = nameof(GetHoldings))]
        public IActionResult GetHoldings(CancellationToken cancellationToken)
        {
            var data = _holdingService.GetHoldings(cancellationToken);
            return new JsonResult(data);
        }
    }
}
