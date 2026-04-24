using ApplicationTracker.Api.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ApplicationTracker.Api.Repository
{
	public class ApplicationContext : DbContext
	{
		private readonly IConfiguration _configuration;
		public DbSet<JobApplication> JobApplications { get; set; } = null!;
		public DbSet<PdfFile> PdfFiles { get; set; } = null!;
		public ApplicationContext(DbContextOptions<ApplicationContext> options, IConfiguration configuration)
			: base(options)
		{
			_configuration = configuration;
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured && _configuration != null)
			{
				var conn = _configuration.GetConnectionString("Postgres");
				if (!string.IsNullOrWhiteSpace(conn))
				{
					optionsBuilder.UseNpgsql(conn);
				}
			}
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{

			// Global DateTime converters to ensure UTC is used for timestamptz columns.
			var dateTimeConverter = new ValueConverter<DateTime, DateTime>(
				v => v.Kind == DateTimeKind.Utc ? v : v.ToUniversalTime(),
				v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

			var nullableDateTimeConverter = new ValueConverter<DateTime?, DateTime?>(
				v => v.HasValue ? (v.Value.Kind == DateTimeKind.Utc ? v.Value : v.Value.ToUniversalTime()) : v,
				v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v);

			foreach (var entityType in modelBuilder.Model.GetEntityTypes())
			{
				var properties = entityType.GetProperties();
				foreach (var property in properties)
				{
					if (property.ClrType == typeof(DateTime))
					{
						property.SetValueConverter(dateTimeConverter);
					}
					else if (property.ClrType == typeof(DateTime?))
					{
						property.SetValueConverter(nullableDateTimeConverter);
					}
				}
			}
			modelBuilder.Entity<PdfFile>().Property(e => e.PdfType).HasConversion(v => v.ToString(), v => (PdfType)Enum.Parse(typeof(PdfType), v));

		}

	}
}
