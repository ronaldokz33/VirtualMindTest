using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtualMind.NetTest.BO.Settings;
using VirtualMind.NetTest.Interfaces;
using VirtualMind.NetTest.VO;

namespace VirtualMind.NetTest.Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExchangeController : Controller
    {
        #region Constructor

        private readonly IPurchase purchaseManager;

        public ExchangeController(IPurchase purchaseManager)
        {
            this.purchaseManager = purchaseManager;
        }

        #endregion

        //[Authorize]
        [HttpPost]
        [Route("purchase")]
        public IActionResult Purchase(Purchase pObject)
        {
            if (pObject == null)
            {
                return BadRequest("Parameters could no be null");
            }

            return Ok(purchaseManager.Insert(pObject));
        }

        //[Authorize]
        [HttpGet]
        [Route("purchases")]
        public ActionResult<IList<Purchase>> Purchases()
        {
            return Ok(purchaseManager.ListForLookup(new Purchase() { }));
        }

        //[Authorize]
        [HttpGet]
        [Route("quote/{currency=}")]
        public ActionResult<Dictionary<string, decimal>> Quote([FromRoute] string currency)
        {
            return Ok(purchaseManager.GetCurrencysQuote(currency));
        }
    }
}
