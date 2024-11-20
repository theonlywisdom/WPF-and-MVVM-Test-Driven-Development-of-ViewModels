using FriendStorage.Model;

namespace FriendStorage.DataAccess
{
    public interface IFriendDataProvider
    {
        Friend GetFriendById(int id);
        void SaveFriend(Friend friend);
        void DeleteFriend(int id);
    }
}