using FriendStorage.UI.Events;
using FriendStorage.UI.ViewModel;
using FriendStorage.UITests.Extensions;
using Moq;

namespace FriendStorage.UITests.ViewModel
{
    public class NavigationItemViewModelTests
    {
        const int _friendId = 7;
        private readonly Mock<IEventAggregator> _eventAggregatorMock;
        private readonly NavigationItemViewModel _viewModel;

        public NavigationItemViewModelTests()
        {
            _eventAggregatorMock = new Mock<IEventAggregator>();

            _viewModel = new NavigationItemViewModel(_friendId, "Yaw", _eventAggregatorMock.Object);
        }

        [Fact(DisplayName = nameof(ShouldPublishOpenFriendEditViewEvent))]
        public void ShouldPublishOpenFriendEditViewEvent()
        {
            const int friendId = 7;
            var eventMock = new Mock<OpenFriendEditViewEvent>();

            _eventAggregatorMock
                .Setup(ea => ea.GetEvent<OpenFriendEditViewEvent>())
                .Returns(eventMock.Object);

            _viewModel.OpenFriendEditViewCommand.Execute(null);

            eventMock.Verify(e => e.Publish(friendId), Times.Once);
        }

        [Fact(DisplayName = nameof(ShouldRaisePropertyChangedEventForDisplayMember))]
        public void ShouldRaisePropertyChangedEventForDisplayMember()
        {
            bool fired = _viewModel.IsPropertyChangedFired(() =>
              {

                  _viewModel.DisplayMember = "Changed";
              }, nameof(_viewModel.DisplayMember));

            Assert.True(fired);
        }
    }
}
