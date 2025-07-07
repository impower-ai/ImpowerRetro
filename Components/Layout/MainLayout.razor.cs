using ImpowerRetro.Components.Controls;
using ImpowerRetro.Components.Utilities;

namespace ImpowerRetro.Components.Layout;

public partial class MainLayout
{
	private Flyout _flyoutRef;
	private string _primaryColor = Constants.App.PrimaryColor;
	private string _secondaryColor = "#764ba2";
	private string _gradientStart = Constants.App.PrimaryColor;
	private string _gradientEnd = "#764ba2";

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
		{
			await MemoryService.InitializeAsync();
			await JSUtilityService.EnableVantaBackground();
		}
	}

	private string GetCurrentSessionId()
	{
		var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
		var segments = uri.Segments;
		if (segments.Length >= 3 && segments[1].Equals("Session/", StringComparison.OrdinalIgnoreCase))
			return segments[2].TrimEnd('/');

		return null;
	}

	protected override void OnInitialized()
	{
		SessionService.UserListChanged += OnUserListChanged;
	}

	private void OnUserListChanged(string sessionId)
	{
		if (_flyoutRef != null && sessionId == GetCurrentSessionId())
			_flyoutRef.Refresh();
	}
}
