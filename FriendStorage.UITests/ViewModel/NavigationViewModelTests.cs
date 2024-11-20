using FriendStorage.UI.ViewModel;

namespace FriendStorage.UITests.ViewModel
{
    public class NavigationViewModelTests
    {
        [Fact]
        public void ShouldLoadFriends()
        {
            var viewModel = new NavigationViewModel(new NavigationDataProviderMock());

            viewModel.Load();

            Assert.Equal(2, viewModel.Friends.Count);

            var friend = viewModel.Friends.SingleOrDefault(f => f.Id == 1);

            Assert.NotNull(friend);
            Assert.Equal("Ama", friend.FirstName);

            friend = viewModel.Friends.SingleOrDefault(f => f.Id == 2);

            Assert.NotNull(friend);
            Assert.Equal("Akua", friend.FirstName);
        }

        [Fact]
        public void ShouldLoadFriendsOnlyOnce()
        {
            var viewModel = new NavigationViewModel(new NavigationDataProviderMock());

            viewModel.Load();
            viewModel.Load();

            Assert.Equal(2, viewModel.Friends.Count);
        }
    }
}
