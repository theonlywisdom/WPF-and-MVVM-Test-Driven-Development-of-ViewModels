using FriendStorage.DataAccess;
using FriendStorage.Model;

namespace FriendStorage.UITests.ViewModel
{
    public class NavigationDataProviderMock : INavigationDataProvider
    {
        public IEnumerable<Friend> GetAllFriends()
        {
            yield return new Friend() { Id = 1, FirstName = "Ama" };
            yield return new Friend() { Id = 2, FirstName = "Akua" };
        }
    }
}
