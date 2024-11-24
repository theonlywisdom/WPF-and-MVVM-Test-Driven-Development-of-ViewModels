using System.Windows;

namespace FriendStorage.UI.Dialogue
{
    /// <summary>
    /// Interaction logic for YesNoDialogue.xaml
    /// </summary>
    public partial class YesNoDialogue : Window
    {
        public YesNoDialogue(string title, string message)
        {
            InitializeComponent();
            Title = title;
            textBlock.Text = message;
        }

        private void ButtonYes_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void ButtonNo_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
