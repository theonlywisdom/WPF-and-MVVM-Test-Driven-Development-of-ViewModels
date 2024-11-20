namespace FriendStorage.UI.ViewModel
{
    public interface IFriendEditViewModel
    {
        void Load(int friendId);
    }
    public class FriendEditViewModel : ViewModelBase, IFriendEditViewModel
    {
        public void Load(int friendId)
        {
            throw new NotImplementedException();
        }
    }
}
