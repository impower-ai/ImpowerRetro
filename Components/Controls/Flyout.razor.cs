using ImpowerRetro.Components.Model;
using ImpowerRetro.Components.Utilities;
using Microsoft.AspNetCore.Components;

namespace ImpowerRetro.Components.Controls;

public partial class Flyout
{
	[Parameter]
	public string SessionID { get; set; }

	[Parameter]
	public Func<Task> ToggleBackgroundAnimations { get; set; }

	private Session Session { get; set; }
	private List<KeyValuePair<string, string>> ActiveUsers { get; set; } = [];

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();
		Session = SessionService.GetSession(SessionID);
		UpdateActiveUsers();
	}

	private void UpdateActiveUsers()
	{
		ActiveUsers = SessionService.GetSessionUsers(SessionID)
									.Select((user, index) => new KeyValuePair<string, string>(user, $"{index + 1}: {user}"))
									.ToList();
	}

	private async Task ClearLocalStorage()
	{
		await MemoryService.DeleteLocalStorage();
		NotificationService.NotifySuccess("Local browser storage has been cleared.");
	}

	public void Refresh()
	{
		UpdateActiveUsers();
		StateHasChanged();
	}

	private void ToggleAnimations()
	{
		ToggleBackgroundAnimations?.Invoke();
		StateHasChanged();
	}
}
