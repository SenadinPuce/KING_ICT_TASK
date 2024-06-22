using API.Helpers;
using Domain.DTOs;
using Domain.Interfaces;
using Domain.SearchObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
	public class UsersController(IUserService userService) : BaseApiController
	{
		private readonly IUserService _userService = userService;

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


		/// <summary>
		/// Gets a list of users.
		/// </summary>
		/// <remarks>
		/// Access: Only accessible by users with the 'admin' role.
		/// </remarks>
		/// <param name="search">The search parameters.</param>
		/// <returns>A list of users.</returns>
		/// <response code="200">Returns the list of users</response>
		/// <response code="401">If the user is unauthorized</response>
		/// <response code="403">If the user is forbidden</response>
		/// <response code="404">If no users are found</response>
		[Cached(600)]
		[Authorize(Policy = "Admin")]
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<PagedResult<AuthResponse>>> Get([FromQuery] BaseSearchObject search)
		{
			var users = await _userService.GetListAsync(search);

			if (users == null) return NotFound();

			return Ok(users);
		}
	}
}
