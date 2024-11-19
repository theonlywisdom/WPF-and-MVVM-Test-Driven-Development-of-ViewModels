using FriendStorage.Model;

namespace FriendStorage.DataAccess
{
    public interface IDataService : IDisposable
    {
        Friend GetFriendById(int friendId);

        void SaveFriend(Friend friend);

        void DeleteFriend(int friendId);

        IEnumerable<Friend> GetAllFriends();
    }
}
