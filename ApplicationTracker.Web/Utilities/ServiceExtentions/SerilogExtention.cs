using Serilog;
using Serilog.Core;

namespace ApplicationTracker.Web.Utilities.ServiceExtentions
{
	public static class SerilogExtention
	{

		public static IHostBuilder AddSerilogEnricher(this IHostBuilder hostBuilder) 
		{

			var levelSwitch = new LoggingLevelSwitch();
			hostBuilder.UseSerilog((context, services, loggerconfig) =>
			{
				// Retrieve IHttpContextAccessor from the host's service provider to pass into the enricher
				var httpContextAccessor = services.GetRequiredService<IHttpContextAccessor>();

				loggerconfig.MinimumLevel.ControlledBy(levelSwitch);

				loggerconfig.Enrich.WithProperty("Application", "JobHuntLogger")
					.Enrich.WithHttpContextEnricher(httpContextAccessor)
					.WriteTo.Seq(context.Configuration.GetValue<string>("Seq:Url")!, apiKey: context.Configuration.GetValue<string>("Seq:ApiKey"), controlLevelSwitch: levelSwitch);
				loggerconfig.WriteTo.Console();
			});
			return hostBuilder;
		}
	}
}
