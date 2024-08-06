using Microsoft.AspNetCore.Mvc;

namespace V2.CarDealer.API.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class UserController : Controller
    {
		[HttpGet]
		public string Vapo()
		{
			return "vapo";
		}
	}
}
