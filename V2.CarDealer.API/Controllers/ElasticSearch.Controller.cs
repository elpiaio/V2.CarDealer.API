using Microsoft.AspNetCore.Mvc;
using V2.CarDealer.API.DTOs;
using V2.CarDealer.API.DTOs.CarsObjects;
using V2.CarDealer.API.Repositories;

namespace V2.CarDealer.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ElasticSearchController : Controller
    { 
        [HttpPut]
        public IActionResult PopulateElasticSearch()
        {
            try
            {
                var result = ElasticSearchRepository.InsertVehicles();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult InsertVehicle([FromBody] Vehicle vehicle)
        {
            try
            {
                var result = ElasticSearchRepository.InsertVehicle(vehicle);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult GetAllVehicles()
        {
            try
            {
                var result = ElasticSearchRepository.GetAllVehicles();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult SearchVehiclesByBrandAndModel(PaginationParam param)
        {
            try
            {
                var result = ElasticSearchRepository.SearchVehiclesByBrandAndModel(param);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
