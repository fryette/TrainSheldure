using Cirrious.CrossCore;
using Trains.Core.Interfaces;
using Trains.Core.Services.Interfaces;
using Trains.Core.ViewModels;
using Trains.UAP.Services;

namespace Trains.UAP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsView
    {
        public SettingsView()
        {
            InitViewModel();
            InitializeComponent();
        }
        private void InitViewModel()
        {
            var model = new SettingsViewModel(Mvx.Resolve<ISerializableService>(), Mvx.Resolve<IAppSettings>(), Mvx.Resolve<IAnalytics>(), Mvx.Resolve<ILocalDataService>(),new UserInteractionService());
            model.Init();
            DataContext = model;
        }
    }
}
