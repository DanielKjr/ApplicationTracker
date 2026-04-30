using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi;

namespace ApplicationTracker.Api.Utility.ServiceExtentions
{
	public static class SwaggerExtension
	{
		public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
		{
			services.AddControllers().AddJsonOptions(o =>
			{
				o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
			});

			services.AddEndpointsApiExplorer();
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "ApplicationTracker.Api", Version = "v1" });
				//kept in case I want to retry required props on the dtos
				//c.SupportNonNullableReferenceTypes();
				//c.UseAllOfToExtendReferenceSchemas();
				// Add JWT Bearer token support for Swagger UI
				c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
				{
					Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
					Name = "Authorization",
					In = ParameterLocation.Header,
					Type = SecuritySchemeType.Http,
					Scheme = "bearer",
					BearerFormat = "JWT"
				});
				
				//Supposedly fixes the enforcement of no null values, even though the property is nullable
			
				c.UseAllOfToExtendReferenceSchemas();
				c.AddSecurityRequirement(document => new OpenApiSecurityRequirement
				{
					[new OpenApiSecuritySchemeReference("Bearer", document)] = []
				});
				
			
				
				//TODO check that this stil works
				//c.AddSecurityRequirement(new OpenApiSecurityRequirement
				//{
				//	{
				//		new OpenApiSecurityScheme
				//		{[]
				//			Reference = new OpenApiReference
				//			{
				//				Type = ReferenceType.SecurityScheme,
				//				Id = JwtBearerDefaults.AuthenticationScheme
				//			}
				//		},
				//		Array.Empty<string>()
				//	}
				//});
			});
			return services;
		}
	}
}
