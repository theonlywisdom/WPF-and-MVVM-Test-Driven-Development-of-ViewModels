using FriendStorage.Model;
using FriendStorage.UI.Events;
using FriendStorage.UI.ViewModel;
using FriendStorage.UI.Wrapper;
using FriendStorage.UITests.Extensions;
using Moq;

namespace FriendStorage.UITests.ViewModel
{
    public class MainViewModelTests
    {
        private Mock<INavigationViewModel> _navigationViewModelMock;
        private Mock<IEventAggregator> _eventAggregatorMock;
        private OpenFriendEditViewEvent _openFriendEditViewEvent;
        private List<Mock<IFriendEditViewModel>> _friendEditViewModelMocks;
        private MainViewModel _viewModel;

        public MainViewModelTests()
        {
            _friendEditViewModelMocks = [];
            _navigationViewModelMock = new Mock<INavigationViewModel>();
            _eventAggregatorMock = new Mock<IEventAggregator>();
            _openFriendEditViewEvent = new OpenFriendEditViewEvent();

            _eventAggregatorMock.Setup(ea => ea.GetEvent<OpenFriendEditViewEvent>())
                .Returns(_openFriendEditViewEvent);

            _viewModel = new MainViewModel(_navigationViewModelMock.Object, CreateFriendEditViewModel, _eventAggregatorMock.Object);
        }

        private IFriendEditViewModel CreateFriendEditViewModel()
        {
            var friendEditViewModelMock = new Mock<IFriendEditViewModel>();
            friendEditViewModelMock.Setup(vm => vm.Load(It.IsAny<int>()))
                .Callback<int>(friendId =>
                {
                    friendEditViewModelMock.Setup(vm => vm.Friend)
                    .Returns(new FriendWrapper(new Friend { Id = friendId }));
                });
            _friendEditViewModelMocks.Add(friendEditViewModelMock);
            return friendEditViewModelMock.Object;
        }

        [Fact(DisplayName = "ShouldCallTheLoadMethodOfTheNavigationViewModel")]
        public void ShouldCallTheLoadMethodOfTheNavigationViewModel()
        {
            _viewModel.Load();

            _navigationViewModelMock.Verify(vm => vm.Load(), Times.Once);
        }

        [Fact(DisplayName = "ShouldAddFriendEditViewModelAndLoadAndSelectIt")]
        public void ShouldAddFriendEditViewModelAndLoadAndSelectIt()
        {
            const int friendId = 7;
            var friendEditViewModelMock = new Mock<IFriendEditViewModel>();

            _openFriendEditViewEvent.Publish(friendId);

            Assert.Single(_viewModel.FriendEditViewModels);

            var friendEditVm = _viewModel.FriendEditViewModels.First();
            Assert.Equal(friendEditVm, _viewModel.SelectedFriendEditViewModel);

            _friendEditViewModelMocks.First().Verify(vm => vm.Load(friendId), Times.Once);
        }

        [Fact]
        public void ShouldAddFriendEditViewModelsOnlyOnce()
        {
            _openFriendEditViewEvent.Publish(5);
            _openFriendEditViewEvent.Publish(5);
            _openFriendEditViewEvent.Publish(6);
            _openFriendEditViewEvent.Publish(7);
            _openFriendEditViewEvent.Publish(7);

            Assert.Equal(3, _viewModel.FriendEditViewModels.Count);
        }

        [Fact]
        public void ShouldRaisePropertyChangedEventForSelectedFriendEditViewModel()
        {
            var friendEditVmMock = new Mock<IFriendEditViewModel>();

            var fired = _viewModel.IsPropertyChangedFired(() =>
            {
                _viewModel.SelectedFriendEditViewModel = friendEditVmMock.Object;
            }, nameof(_viewModel.SelectedFriendEditViewModel));


            Assert.True(fired);
        }

        [Fact(DisplayName = "ShouldRemoveFriendEditViewModelOnCloseFriendTabCommand")]
        public void ShouldRemoveFriendEditViewModelOnCloseFriendTabCommand()
        {
            _openFriendEditViewEvent.Publish(7);

            var friendEditVm = _viewModel.SelectedFriendEditViewModel;

            _viewModel.CloseFriendTabCommand.Execute(friendEditVm);

            Assert.Empty(_viewModel.FriendEditViewModels);
        }
    }
}
