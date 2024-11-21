using FriendStorage.DataAccess;
using FriendStorage.UI.Wrapper;
using System.ComponentModel;
using System.Windows.Input;
using DelegateCommand = FriendStorage.UI.Command.DelegateCommand;

namespace FriendStorage.UI.ViewModel
{
    public interface IFriendEditViewModel
    {
        void Load(int friendId);
        FriendWrapper Friend { get; }
    }
    public class FriendEditViewModel : ViewModelBase, IFriendEditViewModel
    {
        private IFriendDataProvider _dataProvider;
        private FriendWrapper _friend;

        public FriendEditViewModel(IFriendDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
        }

        public ICommand SaveCommand { get; private set; }

        public FriendWrapper Friend
        {
            get => _friend;
            set
            {
                _friend = value;
                OnPropertyChanged();
            }
        }

        public void Load(int friendId)
        {
            var friend = _dataProvider.GetFriendById(friendId);
            Friend = new FriendWrapper(friend);

            Friend.PropertyChanged += Friend_OnPropertyChanged;

            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        private void Friend_OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        private void OnSaveExecute(object obj)
        {
            _dataProvider.SaveFriend(Friend.Model);
        }

        private bool OnSaveCanExecute(object arg)
        {
            return Friend != null && Friend.IsChanged;
        }
    }
}
