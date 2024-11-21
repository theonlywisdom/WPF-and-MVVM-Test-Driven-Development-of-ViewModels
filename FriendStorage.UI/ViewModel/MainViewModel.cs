using FriendStorage.UI.Events;
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
            CloseFriendTabCommand = new DelegateCommand(OnCloseFriendTabExecute);
            _eventAggregator.GetEvent<OpenFriendEditViewEvent>().Subscribe(OnOpenFriendEditView);
        }

        private void OnCloseFriendTabExecute(object obj)
        {
            var friendEditVm = (IFriendEditViewModel)obj;
            FriendEditViewModels.Remove(friendEditVm);
        }

        private void OnOpenFriendEditView(int friendId)
        {
            IFriendEditViewModel? friendEditVm = FriendEditViewModels.SingleOrDefault(
                vm => vm.Friend.Id == friendId);

            if (friendEditVm == null)
            {
                friendEditVm = _friendEditVmCreator();
                FriendEditViewModels.Add(friendEditVm);
                friendEditVm.Load(friendId);
            }

            SelectedFriendEditViewModel = friendEditVm;
        }

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

        public void Load()
        {
            NavigationViewModel.Load();
        }
    }
}
