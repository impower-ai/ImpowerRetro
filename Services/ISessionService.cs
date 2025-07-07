using ImpowerRetro.Components.Model;

namespace ImpowerRetro.Services;

public interface ISessionService
{
	// Properties
	List<SessionTemplate> Templates { get; }

	// Events
	event Action<string> UserListChanged;
	event Action<string, bool> HiddenModeToggled;
	event Action<string, string, string> EmojiReactionReceived;
	event Action<string> SessionEnded;
	event Action<string> RefreshSession;
	event Action<string, string> RefreshSessionColumn;
	event Action<string, int> ColumnChanged;
	event Action<string, TimeSpan> TimerTick;
	event Action<string, bool> TimerEnded;

	// Session Management
	string CreateSession(UserInfo info);
	Session GetSession(string sessionId);
	void EndSession(string sessionId);
	string GenerateReportHtml(string sessionID);

	// User Management
	Task<bool> AddUserToSession(string sessionId, string userName);
	bool IsUserNameTaken(string sessionId, string userName);
	IEnumerable<string> GetSessionUsers(string sessionId);

	// Session Features
	void SendEmojiReaction(string sessionId, string userName, string emoji);
	void ToggleHiddenMode(string sessionId, string userId);

	// Item Management
	List<UserInput> GetItems(string sessionId, string column);
	void AddItem(string sessionId, string content, string userName, string column, bool isWrapUp);
	void ChangeColumn(string sessionId, int columnIndex);
	string GetColumnNote(string sessionId, string column);
	void SetColumnNote(string sessionId, string column, string note);

	// Timer Management
	void StartTimer(string sessionId, TimeSpan duration);
	void StopTimer(string sessionId);
	void ResetTimer(string sessionId, TimeSpan duration);
}
