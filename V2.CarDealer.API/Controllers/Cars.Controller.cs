using Microsoft.AspNetCore.Mvc;
using V2.CarDealer.API.Repositories;

namespace V2.CarDealer.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CarsController : Controller
    {
        [HttpGet]
        public IActionResult GetAll()
        {
            var vehicle = Cars.GetAllCars();
            return Ok(vehicle);
        }

        [HttpGet("{typeId}")]
        public IActionResult GetByType(int typeId)
        {
            var result = Cars.GetByType(typeId);
            return Ok(result);
        }
    }
}