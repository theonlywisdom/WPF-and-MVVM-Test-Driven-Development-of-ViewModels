using FriendStorage.DataAccess;
using FriendStorage.Model;
using System.Collections.ObjectModel;

namespace FriendStorage.UI.ViewModel
{
    public class NavigationViewModel : ViewModelBase
    {
        private INavigationDataProvider _dataProvider;

        public NavigationViewModel(INavigationDataProvider dataProvider)
        {
            Friends = new ObservableCollection<LookupItem>();
            _dataProvider = dataProvider;
        }

        public void Load()
        {
            Friends.Clear();
            foreach (var friend in _dataProvider.GetAllFriends())
            {
                Friends.Add(friend);
            }
        }

        public ObservableCollection<LookupItem> Friends { get; private set; }
    }
}
