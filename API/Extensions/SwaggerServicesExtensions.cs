using Microsoft.OpenApi.Models;
using System.Reflection;

namespace API.Extensions
{
	public static class SwaggerServicesExtensions
	{
		public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
		{
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			services.AddEndpointsApiExplorer();

			services.AddSwaggerGen(c =>
			{
				var info = new OpenApiInfo()
				{
					Title = "API Documentation",
					Version = "v1",
					Description = "Description of API",
					Contact = new OpenApiContact()
					{
						Name = "Senadin Puce",
						Email = "senadin.puce@gmail.com",
					}
				};

				c.SwaggerDoc("v1", info);

				var securitySchema = new OpenApiSecurityScheme
				{
					Name = "JWT Authentication",
					Description = "Enter your JWT token in this field",
					In = ParameterLocation.Header,
					Type = SecuritySchemeType.Http,
					Scheme = "Bearer",
					BearerFormat = "JWT",
					Reference = new OpenApiReference
					{
						Type = ReferenceType.SecurityScheme,
						Id = "Bearer"
					}
				};

				c.AddSecurityDefinition("Bearer", securitySchema);

				var securityRequirement = new OpenApiSecurityRequirement
				{
					{
						securitySchema, new[] {"Bearer"}
					}
				};

				c.AddSecurityRequirement(securityRequirement);

			
				var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
				c.IncludeXmlComments(xmlPath);
			});

			return services;
		}

		public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
		{
			app.UseSwagger(u =>
			{
				u.RouteTemplate = "swagger/{documentName}/swagger.json";
			});

			app.UseSwaggerUI(c =>
			{
				c.RoutePrefix = "swagger";
				c.SwaggerEndpoint(url: "/swagger/v1/swagger.json", name: "KING ICT TASK");
			});

			return app;
		}
	}
}
