using FriendStorage.DataAccess;
using FriendStorage.Model;
using FriendStorage.UI.Events;
using FriendStorage.UI.Wrapper;
using System.ComponentModel;
using System.Windows.Input;
using DelegateCommand = FriendStorage.UI.Command.DelegateCommand;

namespace FriendStorage.UI.ViewModel
{
    public interface IFriendEditViewModel
    {
        void Load(int? friendId);
        FriendWrapper Friend { get; }
    }
    public class FriendEditViewModel : ViewModelBase, IFriendEditViewModel
    {
        private IFriendDataProvider _dataProvider;
        private readonly IEventAggregator _eventAggregator;
        private FriendWrapper _friend;

        public FriendEditViewModel(IFriendDataProvider dataProvider, IEventAggregator eventAggregator)
        {
            _dataProvider = dataProvider;
            _eventAggregator = eventAggregator;
            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
            DeleteCommand = new DelegateCommand(OnDeleteExecute, OnDeleteCanExecute);
        }

        public ICommand SaveCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }

        public FriendWrapper Friend
        {
            get => _friend;
            set
            {
                _friend = value;
                OnPropertyChanged();
            }
        }

        public void Load(int? friendId)
        {
            Friend friend = friendId.HasValue ? _dataProvider.GetFriendById(friendId.Value) : new Friend();

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
            Friend.AcceptChanges();

            _eventAggregator.GetEvent<FriendSavedEvent>().Publish(Friend.Model);
        }

        private bool OnSaveCanExecute(object arg)
        {
            return Friend != null && Friend.IsChanged;
        }

        private void OnDeleteExecute(object obj)
        {
            throw new NotImplementedException();
        }

        private bool OnDeleteCanExecute(object arg)
        {
            return true;
        }
    }
}
