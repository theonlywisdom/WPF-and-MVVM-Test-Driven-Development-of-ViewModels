using FriendStorage.UI.ViewModel;
using Moq;

namespace FriendStorage.UITests.ViewModel
{
    public class MainViewModelTests
    {
        [Fact]
        public void ShouldCallTheLoadMethodOfTheNavigationViewModel()
        {
            var navigationViewModelMock = new Mock<INavigationViewModel>();
            var viewModel = new MainViewModel(navigationViewModelMock.Object);

            viewModel.Load();

            navigationViewModelMock.Verify(vm => vm.Load(), Times.Once);
        }
    }
}
