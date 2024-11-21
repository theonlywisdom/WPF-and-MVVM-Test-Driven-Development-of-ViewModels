using FriendStorage.DataAccess;
using FriendStorage.Model;
using FriendStorage.UI.ViewModel;
using Moq;

namespace FriendStorage.UITests.ViewModel
{
    public class NavigationViewModelTests
    {
        private NavigationViewModel _viewModel;

        public NavigationViewModelTests()
        {
            var eventAggregatorMock = new Mock<IEventAggregator>();
            var navigationDataProviderMock = new Mock<INavigationDataProvider>();
            navigationDataProviderMock.Setup(dp => dp.GetAllFriends())
                .Returns(new List<LookupItem>
                {
                    new LookupItem() { Id = 1, DisplayMember = "Ama" },
                    new LookupItem() { Id = 2, DisplayMember = "Akua" },
                });
            _viewModel = new NavigationViewModel(navigationDataProviderMock.Object, eventAggregatorMock.Object);
        }

        [Fact(DisplayName = nameof(ShouldLoadFriends))]
        public void ShouldLoadFriends()
        {
            _viewModel.Load();

            Assert.Equal(2, _viewModel.Friends.Count);

            var friend = _viewModel.Friends.SingleOrDefault(f => f.Id == 1);

            Assert.NotNull(friend);
            Assert.Equal("Ama", friend.DisplayMember);

            friend = _viewModel.Friends.SingleOrDefault(f => f.Id == 2);

            Assert.NotNull(friend);
            Assert.Equal("Akua", friend.DisplayMember);
        }

        [Fact(DisplayName = nameof(ShouldLoadFriendsOnlyOnce))]
        public void ShouldLoadFriendsOnlyOnce()
        {
            _viewModel.Load();
            _viewModel.Load();

            Assert.Equal(2, _viewModel.Friends.Count);
        }
    }
}
