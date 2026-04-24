using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using System.Security.Claims;

namespace ApplicationTracker.Api.Utility
{
	public class HttpContextEnricher : ILogEventEnricher
	{
		private readonly IHttpContextAccessor _httpContextAccessor;

		public HttpContextEnricher(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
		}

		public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
		{
			var httpContext = _httpContextAccessor.HttpContext;
			if (httpContext == null) return;

			// Prefer standard ClaimTypes.Name, fallback to "name"
			var userName = httpContext.User?.FindFirst(ClaimTypes.Name)?.Value
						   ?? httpContext.User?.FindFirst("name")?.Value
						   ?? "Unknown";

			var logProp = propertyFactory.CreateProperty("User", userName);
			logEvent.AddOrUpdateProperty(logProp);
		}
	}

	public static class HttpContextLoggerExtensions
	{
		
		public static LoggerConfiguration WithHttpContextEnricher(this LoggerEnrichmentConfiguration loggerEnrichmentConfiguration, IHttpContextAccessor httpContextAccessor)
		{
			return loggerEnrichmentConfiguration.With(new HttpContextEnricher(httpContextAccessor));
		}
	}
}
