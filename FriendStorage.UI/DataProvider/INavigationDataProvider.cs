using FriendStorage.Model;

namespace FriendStorage.DataAccess
{
    public interface INavigationDataProvider
    {
        IEnumerable<Friend> GetAllFriends();
    }
}
