using ImpowerRetro.Components.Model;
using ImpowerRetro.Components.Utilities;
using Microsoft.JSInterop;

namespace ImpowerRetro.Services;

public class MemoryService(IJSUtilityService jsUtilityService, ISessionService sessionService) : IMemoryService
{
	public UserInfo UserInfo { get; set; }
	public LocalStorage LocalStorage { get; set; }

	public async Task InitializeAsync()
	{
		await LoadLocalStorage();
	}

	public async Task DeleteLocalStorage()
	{
		await jsUtilityService.DeleteItem(Constants.Storage.UserNameKey);
		await jsUtilityService.DeleteItem(Constants.Storage.TemplateKey);
		await jsUtilityService.DeleteItem(Constants.Storage.TopicKey);
		await jsUtilityService.DeleteItem(Constants.Storage.AnimateKey);
	}

	public async Task UpdateLocalStorage()
	{
		await jsUtilityService.SetItem(Constants.Storage.UserNameKey, UserInfo.UserName);
		await jsUtilityService.SetItem(Constants.Storage.TemplateKey, UserInfo.TemplateName);
		await jsUtilityService.SetItem(Constants.Storage.TopicKey, UserInfo.Topic);
		await jsUtilityService.SetItem(Constants.Storage.AnimateKey, UserInfo.IsAnimatedBackground.ToString());
	}

	public async Task UpdateAnimateBackground(bool animate)
	{
		UserInfo.IsAnimatedBackground = animate;
		if (await jsUtilityService.GetItem(Constants.Storage.UserNameKey) != null)
			await jsUtilityService.SetItem(Constants.Storage.AnimateKey, animate.ToString());
	}

	public async Task HandleRemember(bool remember)
	{
		if (remember)
			await UpdateLocalStorage();
		else
			await DeleteLocalStorage();
	}

	public async Task Ready()
	{
		while (UserInfo == null)
			await Task.Delay(TimeSpan.FromMilliseconds(250));
	}

	private async Task LoadLocalStorage()
	{
		var userName = await jsUtilityService.GetItem(Constants.Storage.UserNameKey);
		var templateName = await jsUtilityService.GetItem(Constants.Storage.TemplateKey);
		var topic = await jsUtilityService.GetItem(Constants.Storage.TopicKey);
		var animate = await jsUtilityService.GetItem(Constants.Storage.AnimateKey);
		
		LocalStorage = new LocalStorage(userName, templateName, topic, animate);
		UserInfo = new UserInfo
		{
			SessionID = UserInfo?.SessionID,
			UserName = LocalStorage.UserName,
			Template = sessionService.Templates.FirstOrDefault(x => x.Name == LocalStorage.TemplateName),
			Topic = LocalStorage.Topic,
			IsAnimatedBackground = string.IsNullOrWhiteSpace(LocalStorage.Animate) || Convert.ToBoolean(LocalStorage.Animate)
		};
	}
}
