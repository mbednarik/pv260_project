using BL.Services.HoldingService;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FundParser.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HoldingController : ControllerBase
    {
        private readonly IHoldingService holdingService;

        public HoldingController(IHoldingService holdingService)
        {
            this.holdingService = holdingService;
        }

        [HttpGet(Name = "GetHoldings")]
        public IActionResult GetHoldings()
        {
            var data = holdingService.GetHoldings();
            return new JsonResult(data);
        }
    }
}
