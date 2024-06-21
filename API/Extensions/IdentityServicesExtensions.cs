using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace API.Extensions
{
    public static class IdentityServicesExtensions
	{
		private static readonly string _keySecret = Environment.GetEnvironmentVariable("TokenKey")
		?? "super secret key added for testing purpose, one must NOT use it in production!";

		public static IServiceCollection AddIdentityServices(this IServiceCollection services)
		{
			services
				.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(options =>
						{
							options.RequireHttpsMetadata = false;
							options.SaveToken = true;
							options.TokenValidationParameters = new TokenValidationParameters
							{
								IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_keySecret)),
								ValidateIssuer = false,
								ValidateAudience = false
							};
						});

			services.AddAuthorization(options =>
			{
				options.AddPolicy("Admin", policy => policy.RequireRole("admin"));
			});

			return services;
		}
	}
}
