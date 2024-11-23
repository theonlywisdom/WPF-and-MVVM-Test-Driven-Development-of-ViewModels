using FriendStorage.DataAccess;
using FriendStorage.Model;
using FriendStorage.UI.Events;
using System.Collections.ObjectModel;

namespace FriendStorage.UI.ViewModel
{
    public class NavigationViewModel : ViewModelBase, INavigationViewModel
    {
        private INavigationDataProvider _dataProvider;
        private readonly IEventAggregator _eventAggregator;

        public NavigationViewModel(INavigationDataProvider dataProvider, IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            Friends = new ObservableCollection<NavigationItemViewModel>();
            _dataProvider = dataProvider;
            _eventAggregator.GetEvent<FriendSavedEvent>().Subscribe(OnFriendSaved);
            _eventAggregator.GetEvent<FriendDeletedEvent>().Subscribe(OnFriendDeleted);
        }

        public ObservableCollection<NavigationItemViewModel> Friends { get; private set; }

        public void Load()
        {
            Friends.Clear();
            foreach (var friend in _dataProvider.GetAllFriends())
            {
                Friends.Add(
                    new NavigationItemViewModel(friend.Id,
                    friend.DisplayMember, _eventAggregator));
            }
        }

        private void OnFriendSaved(Friend friend)
        {
            var displayMember = $"{friend.FirstName} {friend.LastName}";

            var navigationItem = Friends.SingleOrDefault(n => n.Id == friend.Id);
            if (navigationItem != null)
            {
                navigationItem.DisplayMember = displayMember;
            }
            else
            {
                navigationItem = new NavigationItemViewModel(friend.Id, displayMember, _eventAggregator);
                Friends.Add(navigationItem);
            }
        }

        private void OnFriendDeleted(int friendId)
        {
            var navigationItem = Friends.Single(n => n.Id == friendId);
            Friends.Remove(navigationItem);
        }
    }
}
