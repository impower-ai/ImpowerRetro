using ImpowerRetro.Components.Model;
using ImpowerRetro.Components.Utilities;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace ImpowerRetro.Services;

public partial class SessionService : ISessionService
{
	private readonly Dictionary<string, Session> _sessions = new();
	private readonly Dictionary<string, HashSet<string>> _sessionUsers = new();
	private readonly ILogService _logService;

	public SessionService(ILogService logService)
	{
		_logService = logService;
		_ = LoadSessionTemplatesAsync(); // Fire and forget for constructor
	}

	public event Action<string> UserListChanged;
	public event Action<string, bool> HiddenModeToggled;
	public event Action<string, string, string> EmojiReactionReceived;
	public event Action<string> SessionEnded;
	public List<SessionTemplate> Templates { get; private set; }

	public string CreateSession(UserInfo info)
	{
		var session = new Session(info.UserName, info.Template, info.Topic);
		_sessions[session.ID] = session;
		return session.ID;
	}

	public Session GetSession(string sessionId) => _sessions.GetValueOrDefault(sessionId);

	public void SendEmojiReaction(string sessionId, string userName, string emoji)
	{
		if (_sessions.ContainsKey(sessionId))
			EmojiReactionReceived?.Invoke(sessionId, userName, emoji);
	}

	public void ToggleHiddenMode(string sessionId, string userId)
	{
		if (_sessions.TryGetValue(sessionId, out var session) && session.CreatorID == userId)
		{
			session.ToggleHidden();
			HiddenModeToggled?.Invoke(sessionId, session.IsHidden);
		}
	}

	public IEnumerable<string> GetSessionUsers(string sessionId) => _sessionUsers.TryGetValue(sessionId, out var users) ? users : Enumerable.Empty<string>();

	public async Task<bool> AddUserToSession(string sessionId, string userName)
	{
		if (!_sessions.ContainsKey(sessionId))
			return false;

		if (!_sessionUsers.ContainsKey(sessionId))
			_sessionUsers[sessionId] = [];

		if (_sessionUsers[sessionId].Add(userName))
			UserListChanged?.Invoke(sessionId);

		return true;
	}

	public bool IsUserNameTaken(string sessionId, string userName)
	{
		if (sessionId == null)
			return false;

		return _sessionUsers.ContainsKey(sessionId) && _sessionUsers[sessionId].Contains(userName);
	}

	public void EndSession(string sessionId)
	{
		StopTimer(sessionId);
		_sessions.Remove(sessionId);
		_sessionUsers.Remove(sessionId);
		_sessionTimers.Remove(sessionId);
		SessionEnded?.Invoke(sessionId);
	}

	public string GenerateReportHtml(string sessionID)
	{
		var session = GetSession(sessionID);
		var sb = new StringBuilder();
		sb.AppendLine("<!DOCTYPE html>");
		sb.AppendLine("<html lang=\"en\">");
		sb.AppendLine("<head>");
		sb.AppendLine("<meta charset=\"UTF-8\">");
		sb.AppendLine("<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">");
		sb.AppendLine("<title>Retrospective Report</title>");
		sb.AppendLine("<style>");
		sb.AppendLine("body { font-family: Arial, sans-serif; line-height: 1.6; color: #333; max-width: 800px; margin: 0 auto; padding: 20px; }");
		sb.AppendLine("h1, h2 { color: #2c3e50; }");
		sb.AppendLine("table { border-collapse: collapse; width: 100%; margin-bottom: 20px; }");
		sb.AppendLine("th, td { border: 1px solid #ddd; padding: 12px; text-align: left; }");
		sb.AppendLine("th { background-color: #f2f2f2; }");
		sb.AppendLine(".note { background-color: #e8f4f8; border-left: 5px solid #5bc0de; padding: 10px; margin-bottom: 20px; }");
		sb.AppendLine(".wrap-up { background-color: #f0f4c3; border-left: 5px solid #cddc39; padding: 10px; margin-bottom: 20px; }");
		sb.AppendLine("</style>");
		sb.AppendLine("</head>");
		sb.AppendLine("<body>");

		sb.AppendLine("<h1>Retrospective Report</h1>");
		sb.AppendLine($"<p><strong>Session ID:</strong> {session.ID}</p>");
		sb.AppendLine($"<p><strong>Topic:</strong> {(string.IsNullOrEmpty(session.Topic) ? "Not specified" : session.Topic)}</p>");
		sb.AppendLine($"<p><strong>Creator:</strong> {session.CreatorID}</p>");

		foreach (var column in session.Template.Columns)
		{
			sb.AppendLine($"<h2>{column}</h2>");
			var items = session.GetItems(column);
			if (items.Any())
			{
				sb.AppendLine("<table>");
				sb.AppendLine("<tr><th>User</th><th>Timestamp</th><th>Content</th></tr>");
				foreach (var item in items)
				{
					sb.AppendLine("<tr>");
					sb.AppendLine($"<td>{WebUtility.HtmlEncode(item.UserName)}</td>");
					sb.AppendLine($"<td>{item.Timestamp:yyyy-MM-dd HH:mm:ss}</td>");
					sb.AppendLine($"<td>{WebUtility.HtmlEncode(item.Content)}</td>");
					sb.AppendLine("</tr>");
				}

				sb.AppendLine("</table>");
			}
			else
				sb.AppendLine("<p>No items for this column.</p>");

			var note = session.GetColumnNote(column);
			if (!string.IsNullOrEmpty(note))
			{
				sb.AppendLine("<div class=\"note\">");
				sb.AppendLine("<h3>Notes:</h3>");
				sb.AppendLine($"<p>{WebUtility.HtmlEncode(note)}</p>");
				sb.AppendLine("</div>");
			}
		}

		sb.AppendLine("<h2>Wrap Up</h2>");
		if (session.WrapUpItems.Any())
		{
			sb.AppendLine("<div class=\"wrap-up\">");
			sb.AppendLine("<table>");
			sb.AppendLine("<tr><th>User</th><th>Timestamp</th><th>Content</th></tr>");
			foreach (var item in session.WrapUpItems)
			{
				sb.AppendLine("<tr>");
				sb.AppendLine($"<td>{WebUtility.HtmlEncode(item.UserName)}</td>");
				sb.AppendLine($"<td>{item.Timestamp:yyyy-MM-dd HH:mm:ss}</td>");
				sb.AppendLine($"<td>{WebUtility.HtmlEncode(item.Content)}</td>");
				sb.AppendLine("</tr>");
			}

			sb.AppendLine("</table>");
			sb.AppendLine("</div>");
		}
		else
			sb.AppendLine("<p>No wrap-up items.</p>");

		sb.AppendLine("</body>");
		sb.AppendLine("</html>");

		return sb.ToString();
	}

	private async Task LoadSessionTemplatesAsync()
	{
		// Check for custom templates in environment variable first
		var customTemplatesJson = Environment.GetEnvironmentVariable(Constants.App.CustomTemplatesEnvVar);
		
		if (!string.IsNullOrEmpty(customTemplatesJson))
		{
			try
			{
				var customTemplates = JsonConvert.DeserializeObject<List<SessionTemplate>>(customTemplatesJson);
				if (customTemplates?.Any() == true)
				{
					Templates = customTemplates;
					await _logService.LogAsync(LogSource.SessionService, LogLevel.Information, $"Loaded {Templates.Count} custom SessionTemplates from environment variable.");
					return;
				}
			}
			catch (Exception e)
			{
				await _logService.LogAsync(LogSource.SessionService, LogLevel.Warning, $"Failed to parse custom templates from environment variable: {e.Message}. Using defaults.");
			}
		}

		// Fall back to default templates
		Templates = Constants.Templates.DefaultTemplates
			.Select(t => new SessionTemplate(t.Name, t.Categories))
			.ToList();
		await _logService.LogAsync(LogSource.SessionService, LogLevel.Information, $"Loaded {Templates.Count} default SessionTemplates from constants.");
	}
}
