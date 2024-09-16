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
            string result = SalesRepository.ReqCreateSale(sale);
            return Ok(result);
        }

        [HttpGet]
        public IActionResult GetSales()
        {
            var result = SalesRepository.ReqGetSales();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetSaleId(int id)
        {
            var result = SalesRepository.ReqGetSaleId(id);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetSaleUserId(int id)
        {
            var result = SalesRepository.ReqGetSaleUserId(id);
            return Ok(result);
        }

        [HttpGet]
        public IActionResult GetSalesByUsers()
        {
            var result = SalesRepository.ReqGetSalesByUsers();
            return Ok(result);
        }
    }
}
