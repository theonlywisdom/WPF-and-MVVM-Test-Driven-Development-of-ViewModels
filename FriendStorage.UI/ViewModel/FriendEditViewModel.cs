﻿using FriendStorage.DataAccess;
using FriendStorage.Model;

namespace FriendStorage.UI.ViewModel
{
    public interface IFriendEditViewModel
    {
        void Load(int friendId);
        Friend Friend { get; }
    }
    public class FriendEditViewModel : ViewModelBase, IFriendEditViewModel
    {
        private IFriendDataProvider _dataProvider;
        private Friend _friend;

        public FriendEditViewModel(IFriendDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public Friend Friend
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
            Friend = friend; ;
        }
    }
}
