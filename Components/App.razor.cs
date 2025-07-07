using ImpowerRetro.Components.Utilities;
using Microsoft.AspNetCore.Components;

namespace ImpowerRetro.Components;

public partial class App
{
	[CascadingParameter]
	private HttpContext HttpContext { get; set; }

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();

		if (HttpContext != null)
		{
			var theme = HttpContext.Request.Cookies[Constants.App.ThemeCookieName];
			if (!string.IsNullOrEmpty(theme))
				ThemeService.SetTheme(theme, false);
		}
	}
}
