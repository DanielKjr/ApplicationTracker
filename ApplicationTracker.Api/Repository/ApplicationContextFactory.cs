using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ApplicationTracker.Api.Repository
{
	public class ApplicationContextFactory : IDesignTimeDbContextFactory<ApplicationContext>
	{
		public ApplicationContext CreateDbContext(string[] args)
		{
			var config = BuildConfiguration();
			var builder = new DbContextOptionsBuilder<ApplicationContext>().UseNpgsql(config.GetConnectionString("PostgresNoContainer"));
			return new ApplicationContext(builder.Options, config);
		}


		private static IConfigurationRoot BuildConfiguration()
		{

			var relativePath = Path.Combine("docker", "secrets", "dbinfo.json");

			// Search root for the dbinfo file
			var dir = new DirectoryInfo(AppContext.BaseDirectory);
			while (dir != null)
			{
				var candidate = Path.Combine(dir.FullName, relativePath);
				if (File.Exists(candidate))
				{
					return new ConfigurationBuilder().AddJsonFile(candidate, optional: false).Build();
				}
				dir = dir.Parent;
			}

			
			var projectRelative = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", relativePath));
			if (File.Exists(projectRelative))
			{
				return new ConfigurationBuilder().AddJsonFile(projectRelative, optional: false).Build();
			}

			throw new FileNotFoundException(
				$"Could not find configuration file '{relativePath}'. Searched from '{AppContext.BaseDirectory}' and '{Directory.GetCurrentDirectory()}'. " +
				"Place the file in the docker/secrets folder or adjust configuration loading to use an environment variable or copy the file to output.");
		}
	}
}

