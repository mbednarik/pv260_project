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

        [HttpGet(Name = "GetHoldings")]
        public IActionResult GetHoldings()
        {
            var data = _holdingService.GetHoldings();
            return new JsonResult(data);
        }
    }
}
