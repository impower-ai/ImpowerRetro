namespace ImpowerRetro.Components.Utilities;

public static class Constants
{
	public static class App
	{
		public const string ThemeCookieName = "ImpowerRetroTheme";
		public const string PrimaryColor = "#096AF2";
		public const string CustomTemplatesEnvVar = "IR_TEMPLATES";
	}

	public static class Storage
	{
		public const string UserNameKey = "ImpowerRetro.UserName";
		public const string TopicKey = "ImpowerRetro.Topic";
		public const string TemplateKey = "ImpowerRetro.Template";
		public const string AnimateKey = "ImpowerRetro.IsAnimatedBackground";
	}

	public static class UI
	{
		public const string Success = "Success";
		public const string Error = "Error";

		public static readonly Dictionary<string, (string, string)> EmojiDictionary = new()
		{
			{ "👍", ("Thumbs up!", "thumb_up") },
			{ "❤️", ("Love it!", "favorite") },
			{ "😊", ("I'm glad!", "sentiment_satisfied") },
			{ "🎉", ("Let's Party!", "celebration") },
			{ "👏", ("Congratulations!", "handshake") },
			{ "🚀", ("Way to go!", "rocket_launch") },
			{ "🌟", ("You're a Star!", "award_star") },
			{ "🤔", ("Interesting!", "person_raised_hand") },
			{ "👀", ("Whoa!", "visibility") },
			{ "🙌", ("High Five!", "waving_hand") }
		};
	}

	public static class Session
	{
		public const string NotFound = "Session could not be found!";
		public const string SessionCreateError = "Failed to create session!";
		public const string SessionEndedSuccess = "Session ended successfully!";
		public const string TimerFinished = "Time's up!";
		public const string ExportSuccess = "Retrospective exported successfully!";
	}

	public static class JavaScript
	{
		// Function names
		public const string ConfettiStars = "confetti_stars";
		public const string ConfettiFireworks = "confetti_fireworks";
		public const string InitVantaBackground = "initVantaBackground";
		public const string DestroyVantaBackground = "destroyVantaBackground";

		// Vanta config
		public const string VantaElementId = "vanta-background";
	}

	public static class Templates
	{
		public static readonly (string Name, string[] Categories)[] DefaultTemplates =
		[
			("Default", ["Good", "Bad", "Start", "Stop"]),
			("MSG", ["Mad", "Sad", "Glad"]),
			("Sailboat", ["Wind", "Anchors", "Rocks"]),
			("4Ls", ["Liked", "Learned", "Lacked", "Longed For"])
		];
	}

	public static class Styles
	{
		public const string FlexGrowCol = "flex-grow: 1; display: flex; flex-direction: column;";
		public const string FlexGrowRow = "flex-grow: 1; display: flex; flex-direction: row;";
		public const string FlexGrowColSimple = "flex-grow: 1; flex-direction: column;";
		public const string FlexGrowRowSimple = "flex-grow: 1; flex-direction: row;";
		public const string CenterCard = "width: 250px; height: 300px; display: flex; flex-direction: column;";
		public const string IconLarge = "width: 48px; height: 48px; font-size: 48px; color: var(--rz-text-title-color); align-self: center; margin-bottom: 10px;";
		public const string FullWidth = "width: 100%;";

		// Glass card design system
		public const string GlassSessionContainer = "glass-session-container";
		public const string GlassContent = "glass-content";
		public const string GlassControlPanel = "glass-control-panel";

		// Retro-specific glass styles
		public const string RetroWelcomeCard = "retro-welcome-card";
		public const string RetroSessionHeader = "retro-session-header";
		public const string RetroTimerDisplay = "retro-timer-display";

		// Animation classes
		public const string GlassAnimateIn = "glass-animate-in";
	}
}
