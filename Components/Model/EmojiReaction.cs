namespace ImpowerRetro.Components.Model;

public class EmojiReaction
{
	public string Emoji { get; set; }
	public bool IsVisible { get; set; }
	public string PositionStyle { get; set; }
	public string AnimationClass { get; set; }
	public string SizeStyle { get; set; }

	public EmojiReaction(string emoji)
	{
		Emoji = emoji;
		IsVisible = false;
		PositionStyle = "";
		AnimationClass = "";
		SizeStyle = "";
	}
}
