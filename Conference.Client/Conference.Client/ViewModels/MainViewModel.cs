using Prism.Regions;
using Conference.Client.Views;

namespace Conference.Client.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        private readonly IRegionManager _regionManager;

        private const string MainContentFrame = nameof(MainContentFrame);

        public MainViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            _regionManager.RequestNavigate(MainContentFrame, nameof(EnterView));
        }
    }
}
