using ApplicationTracker.Web.Services.HostedServices;

namespace ApplicationTracker.Web.Utilities.ServiceExtentions
{
	public static class TokenCacheExtension
	{
		public static IServiceCollection RegisterTokenCache(this IServiceCollection services, IConfiguration configuration)
		{
			//Entra with SQL distributed token cache
			services.AddDistributedSqlServerCache(options =>
			{
				options.ConnectionString = configuration.GetConnectionString("TokenDb");
				options.SchemaName = "Tokens";
				options.TableName = "TokenCache";
				options.DefaultSlidingExpiration = TimeSpan.FromDays(14);
			});
			services.AddHostedService<TokenCacheCleanupService>();
			return services;
		}
	}
}
