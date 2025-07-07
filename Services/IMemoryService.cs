using ImpowerRetro.Components.Model;

namespace ImpowerRetro.Services;

public interface IMemoryService
{
	// Properties
	UserInfo UserInfo { get; set; }
	LocalStorage LocalStorage { get; set; }

	// Methods
	Task InitializeAsync();
	Task DeleteLocalStorage();
	Task UpdateLocalStorage();
	Task UpdateAnimateBackground(bool animate);
	Task HandleRemember(bool remember);
	Task Ready();
}
