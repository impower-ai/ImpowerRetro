using ImpowerRetro.Components.Controls;
using ImpowerRetro.Components.Model;
using ImpowerRetro.Components.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Radzen;
using Radzen.Blazor;
using System.Diagnostics;

namespace ImpowerRetro.Components.Pages;

public partial class SessionPage : IDisposable
{
	private RadzenSplitButton ReactButton { get; set; }
	private RadzenSteps Steps { get; set; }
	private FloatingReaction FloatingReaction { get; set; }

	private IDisposable _registration;
	private bool _sessionEnding;
	private bool _isVantaBackground = true;

	[Parameter]
	public string SessionID { get; set; }

	private bool IsOwner => CurrentSession.CreatorID == MemoryService.UserInfo.UserName;

	private Session CurrentSession { get; set; }
	private UserInput Input { get; set; } = new();
	private UserInput WrapUpInput { get; set; } = new();

	public void Dispose()
	{
		UnsubscribeFromEvents();
		_registration?.Dispose();
	}

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();

		CurrentSession = SessionService.GetSession(SessionID);
		if (CurrentSession == null)
		{
			NavigationManager.NavigateTo("/");
			return;
		}

		CurrentColumnIndex = CurrentSession.CurrentColumnIndex;
		SubscribeToEvents();
	}

	private void SubscribeToEvents()
	{
		SessionService.RefreshSession += OnRefreshSession;
		SessionService.RefreshSessionColumn += OnRefreshSessionColumn;
		SessionService.HiddenModeToggled += OnHiddenModeToggled;
		SessionService.ColumnChanged += OnColumnChanged;
		SessionService.TimerTick += OnTimerTick;
		SessionService.TimerEnded += OnTimerEnded;
		SessionService.EmojiReactionReceived += OnEmojiReactionReceived;
		SessionService.SessionEnded += OnSessionEnded;
	}

	private void UnsubscribeFromEvents()
	{
		SessionService.RefreshSession -= OnRefreshSession;
		SessionService.RefreshSessionColumn -= OnRefreshSessionColumn;
		SessionService.HiddenModeToggled -= OnHiddenModeToggled;
		SessionService.ColumnChanged -= OnColumnChanged;
		SessionService.TimerTick -= OnTimerTick;
		SessionService.TimerEnded -= OnTimerEnded;
		SessionService.EmojiReactionReceived -= OnEmojiReactionReceived;
		SessionService.SessionEnded -= OnSessionEnded;
	}

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		await base.OnAfterRenderAsync(firstRender);

		if (firstRender)
		{
			if (MemoryService.UserInfo == null)
			{
				await MemoryService.Ready();
				var (success, remember) = await ShowUserNameDialog();
				if (success)
					await MemoryService.HandleRemember(remember);
				else
					NavigationManager.NavigateTo("/");
			}

			_registration ??= NavigationManager.RegisterLocationChangingHandler(async context => await OnLocationChanging(context));
			await SessionService.AddUserToSession(SessionID, MemoryService.UserInfo!.UserName);
			await InvokeAsync(StateHasChanged);
		}
	}

	private async Task OnLocationChanging(LocationChangingContext context)
	{
		if (_sessionEnding)
		{
			_sessionEnding = false;
			return;
		}

		var result = await DialogService.Confirm("Your changes may not be saved. If you're the last participant in your session, it will be deleted!",
												 "Are you sure you want to leave this session?",
												 new ConfirmOptions { OkButtonText = "Leave", CancelButtonText = "Stay" });

		if (!(result.HasValue && result.Value))
			context.PreventNavigation();
	}

	private void OnEmojiReactionReceived(string sessionId, string userName, string emoji)
	{
		if (sessionId == SessionID)
			InvokeAsync(async () =>
			{
				if (FloatingReaction != null)
					await FloatingReaction.ShowReaction(emoji);
			});
	}

	private void OnSessionEnded(string sessionId)
	{
		if (sessionId == SessionID)
		{
			_registration?.Dispose();

			InvokeAsync(async () =>
			{
				await DialogService.Alert("This retrospective session has ended. Thank you for participating!", "Session Complete",
										  new AlertOptions
										  {
											  ShowClose = false,
											  CloseDialogOnEsc = false,
											  CloseDialogOnOverlayClick = false
										  });
				NavigationManager.NavigateTo("/");
			});
		}
	}

	private async void SendReaction(string emoji)
	{
		ReactButton.Close();
		SessionService.SendEmojiReaction(SessionID, MemoryService.UserInfo.UserName, emoji);
	}

	private async Task<(bool Success, bool Remember)> ShowUserNameDialog()
	{
		var result = await DialogService.OpenAsync<UserNameDialog>("Enter Your Name", new Dictionary<string, object>(),
																   new DialogOptions { CloseDialogOnEsc = false, CloseDialogOnOverlayClick = false, ShowClose = false });

		if (result is bool remember)
			return (true, remember);

		return (false, false);
	}

	private async Task CopyToClipboard(RadzenSplitButtonItem args)
	{
		var copyType = Enum.Parse<CopyTypes>(args.Value);

		var data = copyType switch
		{
			CopyTypes.Link => NavigationManager.ToAbsoluteUri($"/Session/{SessionID}").ToString(),
			CopyTypes.ID   => SessionID,
			var _          => throw new ArgumentOutOfRangeException()
		};

		try
		{
			await JSUtilityService.CopyToClipboard(data);
			NotificationService.Notify(new NotificationMessage
			{
				Severity = NotificationSeverity.Success,
				Summary = Constants.UI.Success,
				Detail = $"Session{copyType} copied to clipboard",
				Duration = 4000
			});
		}
		catch (Exception e)
		{
			//This can happen if you're debugging and the focused window is not the browser
			if (!Debugger.IsAttached)
				throw;
		}
	}

	private async Task ExportSession()
	{
		var name = $"Retrospective_Report_{SessionID}";
		var reportHtml = SessionService.GenerateReportHtml(SessionID);

		await JSUtilityService.DownloadHtmlFile(name, reportHtml);
		NotificationService.Notify(NotificationSeverity.Success, Constants.UI.Success, Constants.Session.ExportSuccess);
	}

	private async Task EndSession()
	{
		_sessionEnding = true;
		SessionService.EndSession(SessionID);
		NavigationManager.NavigateTo("/");
		NotificationService.Notify(NotificationSeverity.Success, Constants.UI.Success, Constants.Session.SessionEndedSuccess);
	}

	private async Task OpenSettingsFlyout()
	{
		await DialogService.OpenSideAsync<Flyout>(null, new Dictionary<string, object> { { nameof(Flyout.SessionID), SessionID }, { nameof(Flyout.ToggleBackgroundAnimations), async () => await ToggleVantaBackground() } },
												  new SideDialogOptions
												  {
													  CloseDialogOnOverlayClick = true,
													  Position = DialogPosition.Right,
													  ShowMask = true
												  });
	}

	private async Task ToggleVantaBackground()
	{
		if (_isVantaBackground)
		{
			await JSUtilityService.DisableVantaBackground();
			_isVantaBackground = false;
		}
		else
		{
			await JSUtilityService.EnableVantaBackground();
			_isVantaBackground = true;
		}

		await MemoryService.UpdateAnimateBackground(_isVantaBackground);
	}
}
