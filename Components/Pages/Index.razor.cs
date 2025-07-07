using ImpowerRetro.Components.Controls;
using ImpowerRetro.Components.Model;
using ImpowerRetro.Components.Utilities;
using ImpowerRetro.Services;
using Radzen;

namespace ImpowerRetro.Components.Pages;

public partial class Index
{
	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		await base.OnAfterRenderAsync(firstRender);
		if (firstRender)
			await JSUtilityService.EnableVantaBackground();
	}

	private async Task ShowSessionDialog(DialogModes mode)
	{
		var result = await DialogService.OpenAsync<SessionDialog>(
																  mode == DialogModes.CreateSession ? "Create new Retrospective" : "Join existing Retrospective",
																  new Dictionary<string, object> { { "Mode", mode } },
																  new DialogOptions { Width = "300px", Height = mode == DialogModes.CreateSession ? "430px" : "320px", Resizable = false, Draggable = false });

		if (result is bool remember)
		{
			await MemoryService.HandleRemember(remember);
			NavigationManager.NavigateTo($"/{nameof(Session)}/{MemoryService.UserInfo.SessionID}");
		}
		else if (result != null)
		{
			await LogService.LogAsync(LogSource.UI, LogLevel.Error, "Session creation failed", new { Mode = mode, Result = result });
			NotificationService.NotifyError(Constants.Session.SessionCreateError);
		}
	}
}
