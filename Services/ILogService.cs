using ImpowerRetro.Components.Model;

namespace ImpowerRetro.Services;

public interface ILogService
{
	Task<ServiceResult> LogAsync(LogSource source, LogLevel level, string message, object data = null);
	Task LogServiceResultAsync(ServiceResult result, LogSource source);
	Task LogExceptionAsync(Exception ex, LogSource source, string context = null);
}

public enum LogSource
{
	Application,
	SessionService,
	MemoryService,
	UI,
	JavaScript,
	Timer,
	Authentication,
	Storage
}
