using Radzen;

namespace ImpowerRetro.Components.Utilities;

public static class Extensions
{
	/// <summary>
	///     Creates a success notification
	/// </summary>
	public static void NotifySuccess(this NotificationService service, string message, double duration = 4000)
	{
		service.Notify(NotificationSeverity.Success, Constants.UI.Success, message, duration);
	}

	/// <summary>
	///     Creates an error notification
	/// </summary>
	public static void NotifyError(this NotificationService service, string message, double duration = 4000)
	{
		service.Notify(NotificationSeverity.Error, Constants.UI.Error, message, duration);
	}
}
