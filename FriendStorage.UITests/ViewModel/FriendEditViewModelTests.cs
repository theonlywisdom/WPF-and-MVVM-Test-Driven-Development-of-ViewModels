﻿using FriendStorage.DataAccess;
using FriendStorage.Model;
using FriendStorage.UI.Dialogue;
using FriendStorage.UI.Events;
using FriendStorage.UI.ViewModel;
using FriendStorage.UITests.Extensions;
using Moq;

namespace FriendStorage.UITests.ViewModel
{
    public class FriendEditViewModelTests
    {
        private const int _friendId = 5;
        private Mock<FriendSavedEvent> _friendSavedEventMock;
        private Mock<FriendDeletedEvent> _friendDeletedEventMock;
        private readonly Mock<IEventAggregator> _eventAggregatorMock;
        private readonly Mock<IMessageDialogueService> _messageDialogueServiceMock;
        private Mock<IFriendDataProvider> _dataProviderMock;
        private FriendEditViewModel _viewModel;

        public FriendEditViewModelTests()
        {
            _friendSavedEventMock = new Mock<FriendSavedEvent>();
            _friendDeletedEventMock = new Mock<FriendDeletedEvent>();
            _eventAggregatorMock = new Mock<IEventAggregator>();
            _messageDialogueServiceMock = new Mock<IMessageDialogueService>();
            _eventAggregatorMock.Setup(ea => ea.GetEvent<FriendSavedEvent>())
                .Returns(_friendSavedEventMock.Object);
            _eventAggregatorMock.Setup(ea => ea.GetEvent<FriendDeletedEvent>())
                .Returns(_friendDeletedEventMock.Object);
            _dataProviderMock = new Mock<IFriendDataProvider>();
            _dataProviderMock.Setup(dp => dp.GetFriendById(_friendId))
                .Returns(new Friend
                {
                    Id = _friendId,
                    FirstName = "Nti"
                });

            _viewModel = new FriendEditViewModel(_dataProviderMock.Object, _eventAggregatorMock.Object, _messageDialogueServiceMock.Object);
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

        [Fact(DisplayName = nameof(ShouldRaiseCanExecuteChangedForDeleteCommandAfterLoad))]
        public void ShouldRaiseCanExecuteChangedForDeleteCommandAfterLoad()
        {
            var fired = false;
            _viewModel.DeleteCommand.CanExecuteChanged += (s, e)
                => fired = true;
            _viewModel.Load(_friendId);

            Assert.True(fired);
        }

        [Fact(DisplayName = nameof(ShouldRaiseCanExecuteChangedForDeleteCommandWhenAcceptingChanges))]
        public void ShouldRaiseCanExecuteChangedForDeleteCommandWhenAcceptingChanges()
        {
            _viewModel.Load(_friendId);
            var fired = false;
            _viewModel.Friend.FirstName = "Changed";
            _viewModel.DeleteCommand.CanExecuteChanged += (s, e)
                => fired = true;
            _viewModel.Friend.AcceptChanges();
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

        [Fact(DisplayName = nameof(ShouldCallAccpetChangesWhenSaveCommandIsExecuted))]
        public void ShouldCallAccpetChangesWhenSaveCommandIsExecuted()
        {
            _viewModel.Load(_friendId);
            _viewModel.Friend.FirstName = "Changed";

            _viewModel.SaveCommand.Execute(null);

            Assert.False(_viewModel.Friend.IsChanged);
        }

        [Fact(DisplayName = nameof(ShouldPublishFriendSaveEventWhenSaveCommandIsExecuted))]
        public void ShouldPublishFriendSaveEventWhenSaveCommandIsExecuted()
        {
            _viewModel.Load(_friendId);
            _viewModel.Friend.FirstName = "Changed";

            _viewModel.SaveCommand.Execute(null);
            _friendSavedEventMock.Verify(e => e.Publish(_viewModel.Friend.Model), Times.Once);
        }

        [Fact(DisplayName = nameof(ShouldCreateANewFriendWhenNullIsPassedToLoadMethod))]
        public void ShouldCreateANewFriendWhenNullIsPassedToLoadMethod()
        {
            _viewModel.Load(null);

            Assert.NotNull(_viewModel.Friend);
            Assert.Equal(0, _viewModel.Friend.Id);
            Assert.Null(_viewModel.Friend.FirstName);
            Assert.Null(_viewModel.Friend.LastName);
            Assert.Null(_viewModel.Friend.Birthday);
            Assert.False(_viewModel.Friend.IsDeveloper);

            _dataProviderMock.Verify(dp => dp.GetFriendById(It.IsAny<int>()), Times.Never);
        }

        [Fact(DisplayName = nameof(ShouldEnableDeleteCommandForExistingFriend))]
        public void ShouldEnableDeleteCommandForExistingFriend()
        {
            _viewModel.Load(_friendId);
            Assert.True(_viewModel.DeleteCommand.CanExecute(null));
        }

        [Fact(DisplayName = nameof(ShouldDisbleDeleteCommandForNewFriend))]
        public void ShouldDisbleDeleteCommandForNewFriend()
        {
            _viewModel.Load(null);
            Assert.False(_viewModel.DeleteCommand.CanExecute(null));
        }

        [Fact(DisplayName = nameof(ShouldDisbleDeleteCommandWithoutLoad))]
        public void ShouldDisbleDeleteCommandWithoutLoad()
        {
            Assert.False(_viewModel.DeleteCommand.CanExecute(null));
        }

        [Theory(DisplayName = nameof(ShouldCallDeleteFriendWhenDeleteCommandIsExecuted))]
        [InlineData(MessageDialogueResult.Yes, 1)]
        [InlineData(MessageDialogueResult.No, 0)]
        public void ShouldCallDeleteFriendWhenDeleteCommandIsExecuted(MessageDialogueResult result, int expectedDeleteFriendCalls)
        {
            _viewModel.Load(_friendId);

            _messageDialogueServiceMock.Setup(ds => ds.ShowYesNoDialogue(It.IsAny<string>(), It.IsAny<string>())).Returns(result);

            _viewModel.DeleteCommand.Execute(null);

            _dataProviderMock.Verify(dp => dp.DeleteFriend(_friendId), Times.Exactly(expectedDeleteFriendCalls));

            _messageDialogueServiceMock.Verify(ds => ds.ShowYesNoDialogue(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Theory(DisplayName = nameof(ShouldPublishFriendDeletedEventWhenDeleteCommandIsExecuted))]
        [InlineData(MessageDialogueResult.Yes, 1)]
        [InlineData(MessageDialogueResult.No, 0)]
        public void ShouldPublishFriendDeletedEventWhenDeleteCommandIsExecuted(MessageDialogueResult result, int expectedDeleteFriendCalls)
        {
            _viewModel.Load(_friendId);

            _messageDialogueServiceMock.Setup(ds => ds.ShowYesNoDialogue(It.IsAny<string>(), It.IsAny<string>())).Returns(result);

            _viewModel.DeleteCommand.Execute(null);

            _friendDeletedEventMock.Verify(e => e.Publish(_viewModel.Friend.Model.Id), Times.Exactly(expectedDeleteFriendCalls));

            _messageDialogueServiceMock.Verify(ds => ds.ShowYesNoDialogue(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void ShouldDisplayCorrectMessageInDeleteDialogue()
        {
            _viewModel.Load(_friendId);

            var f = _viewModel.Friend;
            f.FirstName = "Ntim";
            f.LastName = "Nsiah";

            _viewModel.DeleteCommand.Execute(null);

            _messageDialogueServiceMock.Verify(d => d.ShowYesNoDialogue("Delete Friend", $"Do you really want to delete the friend '{f.FirstName} {f.LastName}'"), Times.Once);
        }
    }
}
