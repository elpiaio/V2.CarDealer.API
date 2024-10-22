using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using V2.CarDealer.API.DTOs.UsersObjects;
using V2.CarDealer.API.UserRepository;

namespace V2.CarDealer.API.Controllers
{
    [Route("api/[controller]/[action]")]
	[ApiController]
	public class UserController : Controller
	{
		[HttpPost]
		public IActionResult Login([FromBody] Login login)
		{
			var teste = UserRepositoryClass.MakeLogin(login);
			if (teste != null)
			{
				return Ok(teste);
			}
			else
			{
				return NotFound("No user found");
			}
		}

		[HttpPost]
		public IActionResult Register([FromBody] Register user)
		{
			Regex emailRegex = new Regex(@"^[^\s@]+@[^\s@]+\.[^\s@]+$", RegexOptions.Compiled);
			Regex passwordRegex = new Regex(@"^[A-Za-z\d]{8,}$", RegexOptions.Compiled);

			bool emailValidation = emailRegex.IsMatch(user.Email);
			if (!emailValidation) return ValidationProblem("Invalid email format. Please provide a valid email.");

			bool passwordValidation = passwordRegex.IsMatch(user.Password);
			if (!passwordValidation) return ValidationProblem("Invalid password format. Please provide a valid password.");

			DateTime birthdate;
			if (!DateTime.TryParse(user.Birthdate, out birthdate)) return ValidationProblem("Invalid birthdate format. Please provide a valid date.");

			var teste = UserRepositoryClass.MakeRegister(user, birthdate);
			return Ok(teste);
		}
	}
}
