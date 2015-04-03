using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.WindowsCommon.Platform;
using Windows.UI.Xaml.Controls;
using Trains.Services.Implementations;
using Trains.Services.Interfaces;
using LastRequestTrain = Trains.WP.Implementations.LastRequestTrain;
using Serializable = Trains.WP.Implementations.Serializable;
using Trains.Infrastructure.Interfaces;
using Trains.WP.Implementations;
using Chance.MvvmCross.Plugins.UserInteraction;

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
            Mvx.LazyConstructAndRegisterSingleton<IFileSystem, FileSystem>();
            Mvx.LazyConstructAndRegisterSingleton<ILastRequestTrainService, LastRequestTrain>();
            Mvx.LazyConstructAndRegisterSingleton<ISerializableService, Serializable>();
            Mvx.LazyConstructAndRegisterSingleton<ICheckTrainService, CheckTrain>();
            Mvx.LazyConstructAndRegisterSingleton<IUserInteraction, UserInteractionService>();

            base.InitializePlatformServices();
        }
    }
}