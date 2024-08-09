using Microsoft.AspNetCore.Mvc;
using V2.CarDealer.API.Repositories;
using V2.CarDealer.API.DTOs;

namespace V2.CarDealer.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SalesController : Controller
    {
        [HttpPost]
        public IActionResult CreateSale([FromBody] Sale sale)
        {
            string aaa = SalesRepository.ReqCreateSale(sale);
            return Ok(aaa);
        }

        [HttpGet]
        public IActionResult GetSales()
        {
            var aaa = SalesRepository.ReqGetSales();
            return Ok(aaa);
        }
    }
}
