using FriendStorage.DataAccess;
using FriendStorage.Model;
using FriendStorage.UI.Events;
using FriendStorage.UI.ViewModel;
using Moq;

namespace FriendStorage.UITests.ViewModel
{
    public class NavigationViewModelTests
    {
        private readonly FriendSavedEvent _friendSavedEvent;
        private NavigationViewModel _viewModel;

        public NavigationViewModelTests()
        {
            _friendSavedEvent = new FriendSavedEvent();
            var eventAggregatorMock = new Mock<IEventAggregator>();
            eventAggregatorMock.Setup(ea => ea.GetEvent<FriendSavedEvent>())
                .Returns(_friendSavedEvent);

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

        [Fact(DisplayName = nameof(ShouldUpdateNavigationItemWhenFriendIsSaved))]
        public void ShouldUpdateNavigationItemWhenFriendIsSaved()
        {
            _viewModel.Load();
            var navigationItem = _viewModel.Friends.First();

            var friendId = navigationItem.Id;

            _friendSavedEvent.Publish(
                new Friend
                {
                    Id = friendId,
                    FirstName = "Yaa",
                    LastName = "Poku"
                });

            Assert.Equal("Yaa Poku", navigationItem.DisplayMember);
        }
    }
}
