namespace ImpowerRetro.Components.Model;

public class Session
{
	private readonly Dictionary<string, List<UserInput>> _items = new();
	private readonly Dictionary<string, string> _notes = new();
	public string ID { get; }
	public string CreatorID { get; }
	public SessionTemplate Template { get; }
	public string Topic { get; set; }
	public bool IsHidden { get; internal set; }
	public int CurrentColumnIndex { get; set; }

	public string WrapUpColumn { get; } = "Wrap Up";
	public List<UserInput> WrapUpItems { get; } = [];

	public Session(string creatorId, SessionTemplate template, string topic = null)
	{
		ID = Guid.NewGuid().ToString();
		CreatorID = creatorId;
		Template = template;
		Topic = topic;
		IsHidden = true;
		foreach (var column in template.Columns)
		{
			_items[column] = [];
			_notes[column] = string.Empty;
		}

		_items[WrapUpColumn] = [];
		_notes[WrapUpColumn] = string.Empty;
	}

	public void ToggleHidden()
	{
		IsHidden = !IsHidden;
	}

	public void AddItem(string column, UserInput input)
	{
		if (_items.TryGetValue(column, out var item))
			item.Add(input);
	}

	public List<UserInput> GetItems(string column) => _items.TryGetValue(column, out var items) ? items : [];

	public void SetColumnNote(string column, string note)
	{
		if (_notes.ContainsKey(column))
			_notes[column] = note;
	}

	public string GetColumnNote(string column) => _notes.GetValueOrDefault(column, string.Empty);

	public void AddWrapUpItem(UserInput input)
	{
		WrapUpItems.Add(input);
	}
}
