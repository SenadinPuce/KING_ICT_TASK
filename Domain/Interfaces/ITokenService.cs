﻿using Domain.Entities;

namespace Domain.Interfaces
{
	public interface ITokenService
	{
		string CreateToken(User user);
	}
}
