using FriendStorage.Model;

namespace FriendStorage.DataAccess
{
    public class NavigationDataProvider : INavigationDataProvider
    {
        private readonly Func<IDataService> _dataServiceCreator;

        public NavigationDataProvider(Func<IDataService> dataServiceCreator)
        {
            _dataServiceCreator = dataServiceCreator;
        }
        public IEnumerable<Friend> GetAllFriends()
        {
            using (var dataService = _dataServiceCreator())
            {
                return dataService.GetAllFriends();
            }
        }
    }
}
