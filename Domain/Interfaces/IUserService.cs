using Domain.DTOs;
using Domain.Entities;
using Domain.SearchObjects;

namespace Domain.Interfaces
{
	public interface IUserService : IReadService<User, UserDto, BaseSearchObject>
	{
		Task<AuthResponse?> LoginAsync(LoginDto login);
	}
}
