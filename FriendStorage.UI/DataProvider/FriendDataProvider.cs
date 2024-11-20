using FriendStorage.Model;

namespace FriendStorage.DataAccess
{
    public class FriendDataProvider : IFriendDataProvider
    {
        private readonly Func<IDataService> _dataSerciveCreator;
        public FriendDataProvider(Func<IDataService> dataSerciveCreator)
        {
            _dataSerciveCreator = dataSerciveCreator;
        }

        public Friend GetFriendById(int id)
        {
            using var dataService = _dataSerciveCreator();
            return dataService.GetFriendById(id);
        }

        public void SaveFriend(Friend friend)
        {
            using var dataService = _dataSerciveCreator();
            dataService.SaveFriend(friend);
        }

        public void DeleteFriend(int id)
        {
            using var dataService = _dataSerciveCreator();
            dataService.DeleteFriend(id);
        }
    }
}
