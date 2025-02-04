﻿using FriendStorage.DataAccess;
using FriendStorage.Model;
using FriendStorage.UI.Events;
using FriendStorage.UI.ViewModel;
using Moq;

namespace FriendStorage.UITests.ViewModel
{
    public class NavigationViewModelTests
    {
        private readonly FriendSavedEvent _friendSavedEvent;
        private readonly FriendDeletedEvent _friendDeletedEvent;
        private NavigationViewModel _viewModel;

        public NavigationViewModelTests()
        {
            _friendSavedEvent = new FriendSavedEvent();
            _friendDeletedEvent = new FriendDeletedEvent();
            var eventAggregatorMock = new Mock<IEventAggregator>();
            eventAggregatorMock.Setup(ea => ea.GetEvent<FriendSavedEvent>())
                .Returns(_friendSavedEvent);
            eventAggregatorMock.Setup(ea => ea.GetEvent<FriendDeletedEvent>())
                .Returns(_friendDeletedEvent);

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

        [Fact(DisplayName = nameof(ShouldAddNavigationItemWhenFriendIsSaved))]
        public void ShouldAddNavigationItemWhenFriendIsSaved()
        {
            _viewModel.Load();

            const int newFriendId = 97;

            _friendSavedEvent.Publish(new Friend
            {
                Id = newFriendId,
                FirstName = "Ntekumah",
                LastName = "Ananse"
            });

            Assert.Equal(3, _viewModel.Friends.Count);

            var addedItem = _viewModel.Friends.SingleOrDefault(f => f.Id == newFriendId);
            Assert.NotNull(addedItem);
            Assert.Equal("Ntekumah Ananse", addedItem.DisplayMember);
        }

        [Fact(DisplayName = nameof(ShouldRemoveNavigationItemWhenFriendIsDeleted))]
        public void ShouldRemoveNavigationItemWhenFriendIsDeleted()
        {
            _viewModel.Load();
            var deletedFriendId = _viewModel.Friends.First().Id;
            _friendDeletedEvent.Publish(deletedFriendId);

            Assert.Single(_viewModel.Friends);
            Assert.NotEqual(deletedFriendId, _viewModel.Friends.Single().Id);
        }
    }
}
