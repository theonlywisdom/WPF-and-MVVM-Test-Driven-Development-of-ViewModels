using FriendStorage.UI.Events;
using FriendStorage.UI.ViewModel;
using Moq;

namespace FriendStorage.UITests.ViewModel
{
    public class NavigationItemViewModelTests
    {
        [Fact]
        public void ShouldPublishOpenFriendEditViewEvent()
        {
            const int friendId = 7;
            var eventMock = new Mock<OpenFriendEditViewEvent>();

            var eventAggregatorMock = new Mock<IEventAggregator>();

            eventAggregatorMock
                .Setup(ea => ea.GetEvent<OpenFriendEditViewEvent>())
                .Returns(eventMock.Object);

            var viewModel = new NavigationItemViewModel(friendId, "Yaw", eventAggregatorMock.Object);

            viewModel.OpenFriendEditViewCommand.Execute(null);

            eventMock.Verify(e => e.Publish(friendId), Times.Once);
        }
    }
}
