using FriendStorage.DataAccess;
using FriendStorage.Model;
using FriendStorage.UI.ViewModel;
using FriendStorage.UITests.Extensions;
using Moq;

namespace FriendStorage.UITests.ViewModel
{
    public class FriendEditViewModelTests
    {
        private const int _friendId = 5;
        private Mock<IFriendDataProvider> _dataProviderMock;
        private FriendEditViewModel _viewModel;

        public FriendEditViewModelTests()
        {
            _dataProviderMock = new Mock<IFriendDataProvider>();
            _dataProviderMock.Setup(dp => dp.GetFriendById(_friendId))
                .Returns(new Friend
                {
                    Id = _friendId,
                    FirstName = "Nti"
                });

            _viewModel = new FriendEditViewModel(_dataProviderMock.Object);
        }

        [Fact(DisplayName = nameof(ShouldLoadFriend))]
        public void ShouldLoadFriend()
        {
            _viewModel.Load(_friendId);

            Assert.NotNull(_viewModel.Friend);
            Assert.Equal(_friendId, _viewModel.Friend.Id);

            _dataProviderMock.Verify(dp => dp.GetFriendById(_friendId), Times.Once);
        }

        [Fact(DisplayName = nameof(ShouldRaisePropertyChangedEventForFriend))]
        public void ShouldRaisePropertyChangedEventForFriend()
        {
            var fired = _viewModel.IsPropertyChangedFired(
                () => _viewModel.Load(_friendId),
                nameof(_viewModel.Friend));

            Assert.True(fired);
        }

        [Fact(DisplayName = nameof(ShouldDisableSaveCommandWhenFriendIsLoaded))]
        public void ShouldDisableSaveCommandWhenFriendIsLoaded()
        {
            _viewModel.Load(_friendId);

            Assert.False(_viewModel.SaveCommand.CanExecute(null));
        }

        [Fact(DisplayName = nameof(ShouldEnableSaveCommandWhenFriendIsChanged))]
        public void ShouldEnableSaveCommandWhenFriendIsChanged()
        {
            _viewModel.Load(_friendId);
            _viewModel.Friend.FirstName = "Changed";

            Assert.True(_viewModel.SaveCommand.CanExecute(null));
        }

        [Fact(DisplayName = nameof(ShouldDisableSaveCommandWithoutLoad))]
        public void ShouldDisableSaveCommandWithoutLoad()
        {
            Assert.False(_viewModel.SaveCommand.CanExecute(null));
        }

        [Fact(DisplayName = nameof(ShouldRaiseCanExecuteChangedForSaveCommandWhenFriendIsChanged))]
        public void ShouldRaiseCanExecuteChangedForSaveCommandWhenFriendIsChanged()
        {
            _viewModel.Load(_friendId);

            var fired = false;
            _viewModel.SaveCommand.CanExecuteChanged += (s, e)
                => fired = true;
            _viewModel.Friend.FirstName = "Changed";

            Assert.True(fired);
        }

        [Fact(DisplayName = nameof(ShouldRaiseCanExecuteChangedForSaveCommandAfterLoad))]
        public void ShouldRaiseCanExecuteChangedForSaveCommandAfterLoad()
        {
            var fired = false;
            _viewModel.SaveCommand.CanExecuteChanged += (s, e)
                => fired = true;
            _viewModel.Load(_friendId);

            Assert.True(fired);
        }

        [Fact(DisplayName = nameof(ShouldCallSaveMethodOfDataProviderWhenSaveCommandIsExecuted))]
        public void ShouldCallSaveMethodOfDataProviderWhenSaveCommandIsExecuted()
        {
            _viewModel.Load(_friendId);
            _viewModel.Friend.FirstName = "Changed";

            _viewModel.SaveCommand.Execute(null);
            _dataProviderMock.Verify(dp => dp.SaveFriend(_viewModel.Friend.Model), Times.Once);
        }
    }
}
