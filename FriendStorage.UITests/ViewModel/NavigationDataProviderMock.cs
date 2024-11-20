using FriendStorage.DataAccess;
using FriendStorage.Model;

namespace FriendStorage.UITests.ViewModel
{
    public class NavigationDataProviderMock : INavigationDataProvider
    {
        public IEnumerable<LookupItem> GetAllFriends()
        {
            yield return new LookupItem() { Id = 1, DisplayMember = "Ama" };
            yield return new LookupItem() { Id = 2, DisplayMember = "Akua" };
        }
    }
}
