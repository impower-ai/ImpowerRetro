namespace ImpowerRetro.Components.Model;

public class UserInfo
{
	public string SessionID { get; set; }
	public string UserName { get; set; }
	public string Topic { get; set; }
	public SessionTemplate Template { get; set; }
	public string TemplateName => Template?.Name;
	public bool IsAnimatedBackground { get; set; }
}
