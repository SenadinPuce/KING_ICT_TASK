using Domain.DTOs;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
	public class UsersController : BaseApiController
	{
		private readonly IUserService _userService;

		public UsersController(IUserService userService)
		{
			_userService = userService;
		}


		[HttpPost("login")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public async Task<ActionResult<UserDto>> Login(LoginDto login)
		{
			var userDto = await _userService.LoginAsync(login);

			if (userDto == null) return Unauthorized();

			return Ok(userDto);
		}

	}
}
