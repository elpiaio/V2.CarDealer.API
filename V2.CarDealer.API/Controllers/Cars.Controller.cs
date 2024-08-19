using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using V2.CarDealer.API.CarsRepository;
using V2.CarDealer.API.DTOs;

namespace V2.CarDealer.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                var vehicles = Cars.GetAllCars();
                return Ok(vehicles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public IActionResult InsertVehicle([FromBody] VehicleCreation vehicle)
        {
            if (vehicle == null) return BadRequest("Vehicle data is null");

            try
            {
                var result = Cars.ReqInsertVehicle(vehicle);
                return CreatedAtAction(nameof(GetByType), new { typeId = result.Type_id }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public IActionResult InsertImage([FromBody] ImageForInsert image)
        {
            if (image.Vehicle_Id <= 0 || (image.ImageUrl == "")) return BadRequest("data is not valid");

            try
            {
                Cars.ReqInsertImage(image);
                return Ok("Image inserted successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet]
        public IActionResult GetBrands()
        {
            try
            {
                var result = Cars.GetBrandsRepository();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /* filters of car*/
        [HttpGet("{typeId}")]
        public IActionResult GetByType(int typeId)
        {
            if (typeId <= 0) return BadRequest("Invalid typeId");

            try
            {
                var result = Cars.GetByType(typeId);
                return result != null && result.Any() ? Ok(result) : NotFound("No vehicles found for the given type");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public IActionResult GetByMultiTypes([FromBody] Types TypesList)
        {
            try
            {
                var result = Cars.GetByMultiTypeRepository(TypesList);
                return result != null && result.Any() ? Ok(result) : NotFound("No vehicles found for the given type");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public IActionResult FilterByHP([FromBody] HorsePower horsePower)
        {
            try
            {
                var result = Cars.FilterByHPRepository(horsePower);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
