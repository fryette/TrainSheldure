using Android.Content;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Droid.Platform;
using Cirrious.MvvmCross.ViewModels;
using Trains.Core.Interfaces;
using Trains.Core.Services.Interfaces;
using Trains.Droid.Services;

namespace Trains.Droid
{
    public class Setup : MvxAndroidSetup
    {
        public Setup(Context applicationContext) : base(applicationContext)
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


            Mvx.LazyConstructAndRegisterSingleton<IMarketPlaceService, MarketPlace>();
            Mvx.LazyConstructAndRegisterSingleton<IAnalytics, Analytics>();
			base.InitializePlatformServices();
		}
    }
}