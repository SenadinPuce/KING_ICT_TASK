using Domain.Entities;
using Domain.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Services
{
	public class TokenService : ITokenService
	{
		private readonly string _keySecret = Environment.GetEnvironmentVariable("TokenKey") 
			?? "super secret key added for testing purpose, one must NOT use it in production!";
		private readonly SymmetricSecurityKey _key;

		public TokenService()
		{
			_key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_keySecret));
		}

		public string CreateToken(User user)
		{
			var claims = new List<Claim>
			{
				new(ClaimTypes.Name, user.Username!),
				new(ClaimTypes.Role, user.Role!)
			};

			var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
				Expires = DateTime.Now.AddDays(10),
				SigningCredentials = creds
			};

			var tokenHandler = new JwtSecurityTokenHandler();

			var token = tokenHandler.CreateToken(tokenDescriptor);

			return tokenHandler.WriteToken(token);
		}
	}
}
