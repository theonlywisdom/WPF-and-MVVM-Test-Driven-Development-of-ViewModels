using System.Windows;

namespace FriendStorage.UI.Dialogue
{
    public class MessageDialogueService : IMessageDialogueService
    {
        public MessageDialogueResult ShowYesNoDialogue(string title, string message)
        {
            var result = new YesNoDialogue(title, message)
            {
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = App.Current.MainWindow
            }.ShowDialog().GetValueOrDefault();
            return result == true
                ? MessageDialogueResult.Yes
                : MessageDialogueResult.No;
        }
    }
}
