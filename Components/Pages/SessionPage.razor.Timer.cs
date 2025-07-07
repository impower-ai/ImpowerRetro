using ImpowerRetro.Components.Utilities;
using Radzen;
using Radzen.Blazor;

namespace ImpowerRetro.Components.Pages;

public partial class SessionPage
{
	private bool TimerRunning { get; set; }
	private TimeSpan RemainingTime { get; set; }
	private int CurrentColumnIndex { get; set; }

	private static string FormatTimeSpan(TimeSpan timeSpan) => $"{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";

	private void StartTimer(int duration)
	{
		SessionService.StartTimer(SessionID, TimeSpan.FromMinutes(duration));
		TimerRunning = true;
	}

	private void StopTimer()
	{
		SessionService.StopTimer(SessionID);
		RemainingTime = TimeSpan.Zero;
		TimerRunning = false;
	}

	private void ToggleTimer(RadzenSplitButtonItem args)
	{
		if (TimerRunning)
			StopTimer();
		else
			StartTimer(int.Parse(args.Value));
	}

	private void OnTimerTick(string sessionId, TimeSpan remainingTime)
	{
		if (sessionId == SessionID)
			InvokeAsync(() =>
			{
				RemainingTime = remainingTime;
				TimerRunning = true;
				StateHasChanged();
			});
	}

	private void OnTimerEnded(string sessionId, bool canceled)
	{
		if (sessionId == SessionID)
			InvokeAsync(() =>
			{
				TimerRunning = false;
				RemainingTime = TimeSpan.Zero;
				if (!canceled)
					NotificationService.Notify(NotificationSeverity.Info, "Timer", Constants.Session.TimerFinished);
				StateHasChanged();
			});
	}
}
