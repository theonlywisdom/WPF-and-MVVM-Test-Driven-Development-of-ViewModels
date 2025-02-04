﻿using FriendStorage.UI.Events;
using System.Windows.Input;
using DelegateCommand = FriendStorage.UI.Command.DelegateCommand;

namespace FriendStorage.UI.ViewModel
{
    public class NavigationItemViewModel : ViewModelBase
    {
        private readonly IEventAggregator _eventAggregator;
        private string _displayMember;

        public NavigationItemViewModel(int id, string displayMember, IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            Id = id;
            DisplayMember = displayMember;

            OpenFriendEditViewCommand = new
                DelegateCommand(OnFriendEditViewExecute);
        }

        private void OnFriendEditViewExecute(object friendId)
        {
            _eventAggregator.GetEvent<OpenFriendEditViewEvent>().Publish(Id);
        }

        public int Id { get; private set; }
        public string DisplayMember
        {
            get => _displayMember; set
            {
                _displayMember = value;
                OnPropertyChanged();
            }
        }

        public ICommand OpenFriendEditViewCommand { get; private set; }
    }
}
