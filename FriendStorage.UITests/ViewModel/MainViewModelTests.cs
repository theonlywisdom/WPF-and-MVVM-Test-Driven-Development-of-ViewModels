using FriendStorage.UI.ViewModel;

namespace FriendStorage.UITests.ViewModel
{
    public class MainViewModelTests
    {
        [Fact]
        public void ShouldCallTheLoadMethodOfTheNavigationViewModel()
        {
            var navigationViewModelMock = new NavigationViewModelMock();
            var viewModel = new MainViewModel(navigationViewModelMock);

            viewModel.Load();

            Assert.True(navigationViewModelMock.LoadHasBeenCalled);
        }
    }

    public class NavigationViewModelMock : INavigationViewModel
    {
        public bool LoadHasBeenCalled { get; set; }
        public void Load()
        {
            LoadHasBeenCalled = true;
        }
    }
}
