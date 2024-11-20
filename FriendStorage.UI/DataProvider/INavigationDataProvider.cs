using FriendStorage.Model;

namespace FriendStorage.DataAccess
{
    public interface INavigationDataProvider
    {
        IEnumerable<LookupItem> GetAllFriends();
    }
}
