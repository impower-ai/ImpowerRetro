using Timer = ImpowerRetro.Components.Model.Timer;

namespace ImpowerRetro.Services;

public partial class SessionService
{
	private readonly Dictionary<string, Timer> _sessionTimers = new();
	public event Action<string, TimeSpan> TimerTick;
	public event Action<string, bool> TimerEnded;

	public void StartTimer(string sessionId, TimeSpan duration)
	{
		if (!_sessionTimers.TryGetValue(sessionId, out var timer))
		{
			timer = new Timer();
			_sessionTimers[sessionId] = timer;
		}

		timer.Tick += remainingTime => TimerTick?.Invoke(sessionId, remainingTime);
		timer.TimerEnded += () => TimerEnded?.Invoke(sessionId, false);
		timer.Start(duration);
	}

	public void StopTimer(string sessionId)
	{
		if (_sessionTimers.TryGetValue(sessionId, out var timer))
		{
			timer.Stop();
			TimerEnded?.Invoke(sessionId, true);
		}
	}

	public void ResetTimer(string sessionId, TimeSpan duration)
	{
		if (_sessionTimers.TryGetValue(sessionId, out var timer))
			timer.Reset(duration);
	}
}
