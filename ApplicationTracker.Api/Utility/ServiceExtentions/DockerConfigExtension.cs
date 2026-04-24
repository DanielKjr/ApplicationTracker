namespace ApplicationTracker.Api.Utility.ServiceExtentions
{
	public static class DockerConfigExtension
	{
		public static IConfigurationBuilder AddJsonSecrets(this IConfigurationBuilder builder)
		{
			builder.AddJsonFile("/run/secrets/dbinfo", optional: false, reloadOnChange: true);

			builder.AddJsonFile("/run/secrets/apiinfo", optional: false, reloadOnChange: true);
			return builder;
		}
	}
}
