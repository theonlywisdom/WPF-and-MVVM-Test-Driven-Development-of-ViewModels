﻿using FriendStorage.UI.Events;
using System.Collections.ObjectModel;
using System.Windows.Input;
using DelegateCommand = FriendStorage.UI.Command.DelegateCommand;

namespace FriendStorage.UI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private IFriendEditViewModel? _selectedFriendEditViewModel;
        private Func<IFriendEditViewModel> _friendEditVmCreator;
        private readonly IEventAggregator _eventAggregator;

        public MainViewModel(INavigationViewModel navigationViewModel, Func<IFriendEditViewModel> friendEditVmCreator, IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            NavigationViewModel = navigationViewModel;
            FriendEditViewModels = [];
            _friendEditVmCreator = friendEditVmCreator;
            AddFriendCommand = new DelegateCommand(OnOnAddFriendExecute);
            CloseFriendTabCommand = new DelegateCommand(OnCloseFriendTabExecute);
            _eventAggregator.GetEvent<OpenFriendEditViewEvent>().Subscribe(OnOpenFriendEditView); _eventAggregator.GetEvent<FriendDeletedEvent>().Subscribe(OnDeleted);
        }

        public ICommand AddFriendCommand { get; private set; }
        public ICommand CloseFriendTabCommand { get; private set; }

        public INavigationViewModel NavigationViewModel { get; private set; }

        public ObservableCollection<IFriendEditViewModel> FriendEditViewModels { get; private set; }


        public IFriendEditViewModel? SelectedFriendEditViewModel
        {
            get { return _selectedFriendEditViewModel; }
            set
            {
                _selectedFriendEditViewModel = value;
                OnPropertyChanged();
            }
        }

        private void OnCloseFriendTabExecute(object obj)
        {
            var friendEditVm = (IFriendEditViewModel)obj;
            FriendEditViewModels.Remove(friendEditVm);
        }

        private void OnDeleted(int friendId)
        {
            var friendEditVm = FriendEditViewModels.Single(f => f.Friend.Id == friendId);
            FriendEditViewModels.Remove(friendEditVm);
        }

        private void OnOnAddFriendExecute(object obj)
        {
            SelectedFriendEditViewModel = CreateAndLoadFriendEditViewModel(null);
        }

        private void OnOpenFriendEditView(int friendId)
        {
            IFriendEditViewModel? friendEditVm = FriendEditViewModels.SingleOrDefault(
                vm => vm.Friend.Id == friendId);

            if (friendEditVm == null)
            {
                friendEditVm = CreateAndLoadFriendEditViewModel(friendId);
            }

            SelectedFriendEditViewModel = friendEditVm;
        }

        private IFriendEditViewModel CreateAndLoadFriendEditViewModel(int? friendId)
        {
            var friendEditVm = _friendEditVmCreator();
            FriendEditViewModels.Add(friendEditVm);
            friendEditVm.Load(friendId);
            return friendEditVm;
        }

        public void Load()
        {
            NavigationViewModel.Load();
        }
    }
}
