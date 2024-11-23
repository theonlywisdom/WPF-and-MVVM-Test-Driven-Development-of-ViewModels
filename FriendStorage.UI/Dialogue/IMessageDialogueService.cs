namespace FriendStorage.UI.Dialogue
{
    public interface IMessageDialogueService
    {
        MessageDialogueResult ShowYesNoDialogue(string title, string message);
    }

    public enum MessageDialogueResult
    {
        Yes,
        No
    }
}
