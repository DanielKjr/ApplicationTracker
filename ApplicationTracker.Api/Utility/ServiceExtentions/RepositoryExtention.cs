using ApplicationTracker.Api.Repository;
using DK.GenericLibrary.ServiceCollection;
using Microsoft.EntityFrameworkCore.Design;

namespace ApplicationTracker.Api.Utility.ServiceExtentions
{
	public static class RepositoryExtention
	{
		public static IServiceCollection RegisterRepository(this IServiceCollection services)
		{
			services.AddTransientAsyncRepository<ApplicationContext>();

			services.AddDbContextFactory<ApplicationContext>();
			services.AddTransient<IDesignTimeDbContextFactory<ApplicationContext>, ApplicationContextFactory>();
			return services;
		}
	}
}
