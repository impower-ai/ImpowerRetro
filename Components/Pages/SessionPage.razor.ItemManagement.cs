using ImpowerRetro.Components.Model;
using Radzen;

namespace ImpowerRetro.Components.Pages;

public partial class SessionPage
{
	private string _columnNote;
	private bool IsHidden => CurrentSession?.IsHidden ?? true;

	private string ColumnNote
	{
		get => SessionService.GetColumnNote(SessionID, CurrentColumn);
		set
		{
			if (_columnNote != value)
			{
				_columnNote = value;
				UpdateColumnNote(CurrentColumn, value);
			}
		}
	}

	private string CurrentColumn => CurrentSession.Template.Columns[CurrentColumnIndex];

	private void OnRefreshSession(string sessionId)
	{
		if (sessionId == SessionID)
			InvokeAsync(StateHasChanged);
	}

	private void OnRefreshSessionColumn(string sessionId, string column)
	{
		if (sessionId == SessionID && column == CurrentColumn)
			InvokeAsync(StateHasChanged);
	}

	private void UpdateColumnNote(string column, string note)
	{
		if (IsOwner)
			SessionService.SetColumnNote(SessionID, column, note);
	}

	private void ToggleHiddenMode()
	{
		if (CurrentSession.CreatorID == MemoryService.UserInfo.UserName)
			SessionService.ToggleHiddenMode(SessionID, MemoryService.UserInfo.UserName);
	}

	private void OnColumnChanged(string sessionId, int columnIndex)
	{
		if (sessionId == SessionID)
			InvokeAsync(() =>
			{
				CurrentColumnIndex = columnIndex;
				StateHasChanged();
			});
	}

	private void OnStepsIndexChange(int index)
	{
		if (!IsOwner)
			return;

		if (!IsHidden)
			ToggleHiddenMode();

		SessionService.ChangeColumn(SessionID, index);
	}

	private void OnHiddenModeToggled(string sessionId, bool isHidden)
	{
		if (sessionId == SessionID)
			InvokeAsync(() =>
			{
				CurrentSession.IsHidden = isHidden;
				StateHasChanged();
			});
	}

	private void OnInputSubmit()
	{
		SessionService.AddItem(SessionID, Input.Content, MemoryService.UserInfo.UserName, CurrentColumn, false);
		Input = new UserInput();
	}

	private void OnWrapUpInputSubmit()
	{
		SessionService.AddItem(SessionID, WrapUpInput.Content, MemoryService.UserInfo.UserName, null, true);
		WrapUpInput = new UserInput();
	}

	private void OnInvalidInputSubmit(FormInvalidSubmitEventArgs obj) { }
	private void OnInvalidWrapUpInputSubmit(FormInvalidSubmitEventArgs obj) { }

	private void AppendToNote(UserInput input)
	{
		ColumnNote += $"Input by {input.UserName}:\r\n{input.Content}\r\n\r\n";
	}
}
