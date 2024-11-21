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

        [Fact(DisplayName = "ShouldLoadFriend")]
        public void ShouldLoadFriend()
        {
            _viewModel.Load(_friendId);

            Assert.NotNull(_viewModel.Friend);
            Assert.Equal(_friendId, _viewModel.Friend.Id);

            _dataProviderMock.Verify(dp => dp.GetFriendById(_friendId), Times.Once);
        }

        [Fact(DisplayName = "ShouldRaisePropertyChangedEventForFriend")]
        public void ShouldRaisePropertyChangedEventForFriend()
        {
            var fired = _viewModel.IsPropertyChangedFired(
                () => _viewModel.Load(_friendId),
                nameof(_viewModel.Friend));

            Assert.True(fired);
        }
    }
}
