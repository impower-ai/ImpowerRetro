using ImpowerRetro.Components.Model;

namespace ImpowerRetro.Services;

public partial class SessionService
{
	public event Action<string> RefreshSession;
	public event Action<string, string> RefreshSessionColumn;
	public event Action<string, int> ColumnChanged;

	public List<UserInput> GetItems(string sessionId, string column) => _sessions.TryGetValue(sessionId, out var session) ? session.GetItems(column) : [];

	public void AddItem(string sessionId, string content, string userName, string column, bool isWrapUp)
	{
		if (!_sessions.TryGetValue(sessionId, out var session))
			return;

		var userInput = new UserInput
		{
			UserName = userName,
			Content = content,
			Timestamp = DateTime.Now
		};

		if (isWrapUp)
			session.AddWrapUpItem(userInput);
		else
			session.AddItem(column, userInput);

		RefreshSession?.Invoke(sessionId);
	}

	public void ChangeColumn(string sessionId, int columnIndex)
	{
		if (_sessions.TryGetValue(sessionId, out var session))
		{
			session.CurrentColumnIndex = columnIndex;
			ColumnChanged?.Invoke(sessionId, columnIndex);
		}
	}

	public string GetColumnNote(string sessionId, string column) => _sessions.TryGetValue(sessionId, out var session) ? session.GetColumnNote(column) : string.Empty;

	public void SetColumnNote(string sessionId, string column, string note)
	{
		if (_sessions.TryGetValue(sessionId, out var session))
		{
			session.SetColumnNote(column, note);
			RefreshSessionColumn?.Invoke(sessionId, column);
		}
	}
}
