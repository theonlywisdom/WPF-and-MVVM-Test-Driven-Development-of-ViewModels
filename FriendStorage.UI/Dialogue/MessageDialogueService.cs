using System.Windows;

namespace FriendStorage.UI.Dialogue
{
    public class MessageDialogueService : IMessageDialogueService
    {
        public MessageDialogueResult ShowYesNoDialogue(string title, string message)
        {
            var result = MessageBox.Show(message, title, MessageBoxButton.YesNo);
            return result == MessageBoxResult.Yes
                ? MessageDialogueResult.Yes
                : MessageDialogueResult.No;
        }
    }
}
