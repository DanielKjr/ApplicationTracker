using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace ApplicationTracker.Api.Utility
{
	/// <summary>
	/// Used to add JWT support to scalar interface.
	/// </summary>
	/// <param name="authenticationSchemeProvider"></param>
	internal sealed class BearerSecuritySchemeTransformer(
	IAuthenticationSchemeProvider authenticationSchemeProvider
) : IOpenApiDocumentTransformer
	{
		public async Task TransformAsync(
			OpenApiDocument document,
			OpenApiDocumentTransformerContext context,
			CancellationToken cancellationToken)
		{
			var schemes = await authenticationSchemeProvider.GetAllSchemesAsync();

			if (!schemes.Any(s => s.Name == "Bearer"))
				return;

			document.Components ??= new OpenApiComponents();

			if (document.Components.SecuritySchemes == null)
				document.Components.SecuritySchemes = new Dictionary<string, IOpenApiSecurityScheme>();

			var schemeId = "Bearer";

			document.Components.SecuritySchemes[schemeId] = new OpenApiSecurityScheme
			{
				Type = SecuritySchemeType.Http,
				Scheme = "bearer",
				BearerFormat = "JWT",
				In = ParameterLocation.Header,
				Description = "JWT Authorization"
			};

			document.Security ??= new List<OpenApiSecurityRequirement>();

			document.Security.Add(new OpenApiSecurityRequirement
			{
				[new OpenApiSecuritySchemeReference(schemeId)] = new List<string>()
			});
		}
	}
}
