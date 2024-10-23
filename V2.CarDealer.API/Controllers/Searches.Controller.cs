using Microsoft.AspNetCore.Mvc;
using V2.CarDealer.API.DTOs;
using V2.CarDealer.API.DTOs.CarsObjects;
using V2.CarDealer.API.Repositories;

namespace V2.CarDealer.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class Searches : ControllerBase
    {
        [HttpPost]
        public IActionResult GlobalSearch([FromBody] PaginationParam paginationParam)
        {
            try 
            {
                var result = SearchesRepository.GlobalSearchRep(paginationParam);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public IActionResult SearchByBrand([FromBody] PaginationParam paginationParam)
        {
            try
            {
                var result = SearchesRepository.SearchByBrand(paginationParam);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public IActionResult SearchByModel([FromBody] PaginationParam paginationParam)
        {
            try
            {
                var result = SearchesRepository.SearchByModel(paginationParam);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
