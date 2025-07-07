namespace ImpowerRetro.Components.Model;

public class SessionTemplate
{
	public string Name { get; set; }
	public List<string> Columns { get; set; }

	public SessionTemplate(string name, params string[] columns)
	{
		Name = name;
		Columns = [.. columns];
	}
}
