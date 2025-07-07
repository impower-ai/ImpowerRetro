namespace ImpowerRetro.Components.Model;

public class LocalStorage(string userName, string templateName, string topic, string animate)
{
	public string UserName { get; set; } = userName;
	public string TemplateName { get; set; } = templateName;
	public string Topic { get; set; } = topic;
	public string Animate { get; set; } = animate;
}
