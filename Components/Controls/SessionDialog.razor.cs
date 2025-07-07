using ImpowerRetro.Components.Model;
using ImpowerRetro.Components.Utilities;
using ImpowerRetro.Services;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace ImpowerRetro.Components.Controls;

public partial class SessionDialog
{
	[Parameter]
	public DialogModes Mode { get; set; }

	private bool Remember { get; set; }
	private bool IsBusy { get; set; }


	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();
		Remember = !string.IsNullOrWhiteSpace(MemoryService.UserInfo?.UserName);
	}

	private async Task OnSubmit()
	{
		IsBusy = true;
		try
		{
			switch (Mode)
			{
				case DialogModes.CreateSession:
				{
					var sessionID = SessionService.CreateSession(MemoryService.UserInfo);
					MemoryService.UserInfo.SessionID = sessionID;
					DialogService.Close(Remember);
					break;
				}

				case DialogModes.JoinSession:
				{
					var session = SessionService.GetSession(MemoryService.UserInfo.SessionID);
					if (session != null)
						DialogService.Close(Remember);
					else
					{
						await LogService.LogAsync(LogSource.SessionService, LogLevel.Warning, $"Session not found: {MemoryService.UserInfo.SessionID}");
						NavigationManager.NavigateTo("/");
						NotificationService.NotifyError(Constants.Session.NotFound);
					}

					break;
				}
			}
		}
		finally
		{
			IsBusy = false;
		}
	}

	private void OnInvalidSubmit(FormInvalidSubmitEventArgs args) { }
}
