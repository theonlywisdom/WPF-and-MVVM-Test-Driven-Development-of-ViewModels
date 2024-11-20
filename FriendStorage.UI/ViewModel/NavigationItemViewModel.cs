using System.Windows.Input;
using DelegateCommand = FriendStorage.UI.Command.DelegateCommand;

namespace FriendStorage.UI.ViewModel
{
    public class NavigationItemViewModel : ViewModelBase
    {
        private readonly IEventAggregator _eventAggregator;
        public NavigationItemViewModel(int id, string displayMember, IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            Id = id;
            DisplayMember = displayMember;

            OpenFriendEditViewCommand = new
                DelegateCommand(OnFriendEditViewExecute);
        }

        private void OnFriendEditViewExecute(object friendId)
        {
            throw new NotImplementedException();
        }

        public int Id { get; private set; }
        public string DisplayMember { get; private set; }

        public ICommand OpenFriendEditViewCommand { get; private set; }
    }
}
