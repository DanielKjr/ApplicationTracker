using Microsoft.Data.SqlClient;
using System.Data;

namespace ApplicationTracker.Web.Services.HostedServices
{
	public class TokenCacheCleanupService(ILogger<TokenCacheCleanupService> _logger, IConfiguration _configuration) : BackgroundService
	{
		private readonly TimeSpan _interval = TimeSpan.FromHours(24);

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			while(!stoppingToken.IsCancellationRequested)
			{
				try
				{
					await CleanupExpiredEntriesAsync(stoppingToken);
				}
				catch(Exception ex)
				{
					_logger.LogError(ex, "Error during token cache cleanup");
				}
				await Task.Delay(_interval, stoppingToken);
			}
		}

		private async Task CleanupExpiredEntriesAsync(CancellationToken ct)
		{

			//const string sql = "DELETE FROM [Tokens].[TokenCache] WHERE ExpiresAtTime < SYSUTCDATETIMEOFFSET()";
			const string sql = "DELETE FROM [Tokens].[TokenCache] WHERE ExpiresAtTime < SYSDATETIMEOFFSET()";

			await using var conn = new SqlConnection(_configuration.GetConnectionString("TokenDb"));
			await conn.OpenAsync(ct);
			await using var cmd = new SqlCommand(sql, conn)
			{
				CommandType = CommandType.Text
			};
			var rows = await cmd.ExecuteNonQueryAsync(ct);
			if(rows > 0)
				_logger.LogWarning("Token cache cleanup removed {Rows} rows", rows);
		}
	}
}
