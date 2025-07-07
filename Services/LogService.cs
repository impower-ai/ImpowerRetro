using ImpowerRetro.Components.Model;
using ImpowerRetro.Components.Utilities;
using System.Text.Json;

namespace ImpowerRetro.Services;

public class LogService : ILogService
{
	private readonly ILogger<LogService> _logger;

	public LogService(ILogger<LogService> logger)
	{
		_logger = logger;
	}

	public Task<ServiceResult> LogAsync(LogSource source, LogLevel level, string message, object data = null)
	{
		try
		{
			var formattedMessage = $"[{source}] {message}";
			if (data != null)
				formattedMessage += $" | Data: {JsonSerializer.Serialize(data)}";

			switch (level)
			{
				case LogLevel.Critical:
					_logger.LogCritical(formattedMessage);
					break;

				case LogLevel.Error:
					_logger.LogError(formattedMessage);
					break;

				case LogLevel.Warning:
					_logger.LogWarning(formattedMessage);
					break;

				case LogLevel.Information:
					_logger.LogInformation(formattedMessage);
					break;

				case LogLevel.Debug:
					_logger.LogDebug(formattedMessage);
					break;

				case LogLevel.Trace:
					_logger.LogTrace(formattedMessage);
					break;

				default:
					_logger.LogInformation(formattedMessage);
					break;
			}

			return Task.FromResult(ServiceResult.Success("Log entry created"));
		}
		catch (Exception ex)
		{
			// Fallback to console if logger fails
			Console.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}] [{level}] [{source}] {message}");
			Console.WriteLine($"Logger error: {ex.Message}");
			return Task.FromResult(ServiceResult.Failure($"Logging failed: {ex.Message}"));
		}
	}

	public async Task LogServiceResultAsync(ServiceResult result, LogSource source)
	{
		var level = result.Successful ? LogLevel.Information : LogLevel.Error;
		var message = result.Successful ? $"Operation succeeded: {result.Message}" : $"Operation failed: {result.Message}";
		await LogAsync(source, level, message);
	}

	public async Task LogExceptionAsync(Exception ex, LogSource source, string context = null)
	{
		var message = string.IsNullOrWhiteSpace(context)
			? $"Exception occurred: {ex.Message}"
			: $"Exception in {context}: {ex.Message}";

		await LogAsync(source, LogLevel.Error, message, new
		{
			ExceptionType = ex.GetType().Name,
			ex.StackTrace,
			InnerException = ex.InnerException?.Message
		});
	}
}
