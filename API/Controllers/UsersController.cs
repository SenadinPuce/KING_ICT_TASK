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



		/// <summary>
		/// Logs in a user.
		/// </summary>
		/// <remarks>
		/// Access: Accessible by anonymous users. This endpoint is used for logging in a user with a username and password.
		/// </remarks>
		/// <param name="login">The login credentials (username and password).</param>
		/// <returns>A UserDto containing the username, email, and JWT token.</returns>
		/// <response code="200">Returns the UserDto with the username, email, and token</response>
		/// <response code="401">If the login credentials are invalid</response>
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
