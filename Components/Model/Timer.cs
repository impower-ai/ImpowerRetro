using System.Timers;

namespace ImpowerRetro.Components.Model;

public class Timer
{
	private readonly System.Timers.Timer _timer;
	public TimeSpan RemainingTime { get; private set; }
	public bool IsRunning { get; private set; }

	public Timer()
	{
		_timer = new System.Timers.Timer(1000);
		_timer.Elapsed += OnTimerElapsed;
	}

	public event Action<TimeSpan> Tick;
	public event Action TimerEnded;

	public void Start(TimeSpan duration)
	{
		RemainingTime = duration;
		IsRunning = true;
		_timer.Start();
	}

	public void Stop()
	{
		_timer.Stop();
		IsRunning = false;
	}

	public void Reset(TimeSpan duration)
	{
		Stop();
		RemainingTime = duration;
	}

	private void OnTimerElapsed(object sender, ElapsedEventArgs e)
	{
		RemainingTime = RemainingTime.Subtract(TimeSpan.FromSeconds(1));
		Tick?.Invoke(RemainingTime);

		if (RemainingTime <= TimeSpan.Zero)
		{
			Stop();
			TimerEnded?.Invoke();
		}
	}
}
