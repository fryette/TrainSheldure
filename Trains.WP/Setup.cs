using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.WindowsCommon.Platform;
using Windows.UI.Xaml.Controls;
using Trains.Services.Implementations;
using Trains.Services.Interfaces;
using Chance.MvvmCross.Plugins.UserInteraction;
using Trains.WP.Services;
using Trains.Core.Interfaces;

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

            base.InitializePlatformServices();
        }
    }
}