namespace ApplicationTracker.Web.Utilities.ServiceExtentions
{
	public static class LoggingExtention
	{
	
		public static IServiceCollection AddLoggingConfiguration(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddLogging(builder =>
			{
				builder.ClearProviders();
				builder.AddConsole();
				builder.AddDebug();
				builder.AddConfiguration(configuration.GetSection("Logging"));
				
			});
			return services;
		}
	}
}
