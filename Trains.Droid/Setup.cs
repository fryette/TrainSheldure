using Android.Content;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Droid.Platform;
using Cirrious.MvvmCross.ViewModels;
using Trains.Droid.Services;
using Trains.Infrastructure.Interfaces;

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
			Mvx.LazyConstructAndRegisterSingleton<ISerializableService, Serialize>();

			base.InitializePlatformServices();
		}
    }
}