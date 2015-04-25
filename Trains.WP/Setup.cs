using Windows.UI.Xaml.Controls;
using Chance.MvvmCross.Plugins.UserInteraction;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.WindowsCommon.Platform;
using Trains.Core.Interfaces;
using Trains.Services.Interfaces;
using Trains.WP.Services;

namespace Trains.WP
{
    public class Setup : MvxWindowsSetup
    {
        public Setup(Frame rootFrame)
            : base(rootFrame)
        {
        }

        protected override IMvxApplication CreateApp()
        {
            return new Core.App();
        }

        protected override IMvxTrace CreateDebugTrace()
        {
            return new DebugTrace();
        }

        protected override void InitializePlatformServices()
        {
            Mvx.LazyConstructAndRegisterSingleton<ISerializableService, Serialize>();
            Mvx.LazyConstructAndRegisterSingleton<IUserInteraction, UserInteractionService>();
            Mvx.LazyConstructAndRegisterSingleton<IMarketPlaceService, MarketPlace>();
            Mvx.LazyConstructAndRegisterSingleton<IAnalytics, Analytics>();
            Mvx.LazyConstructAndRegisterSingleton<IManageLangService, ManageLang>();

            base.InitializePlatformServices();
        }
    }
}