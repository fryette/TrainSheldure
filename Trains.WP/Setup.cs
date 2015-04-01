using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.WindowsCommon.Platform;
using Windows.UI.Xaml.Controls;
using Trains.Services.Implementations;
using Trains.Services.Interfaces;
using FavoriteManage = Trains.WP.Implementations.FavoriteManage;
using LastRequestTrain = Trains.WP.Implementations.LastRequestTrain;
using Search = Trains.WP.Implementations.Search;
using Serializable = Trains.WP.Implementations.Serializable;
using Start = Trains.WP.Implementations.Start;

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
            Mvx.LazyConstructAndRegisterSingleton<ILastRequestTrainService, LastRequestTrain>();
            Mvx.LazyConstructAndRegisterSingleton<IStartService, Start>();
            Mvx.LazyConstructAndRegisterSingleton<ISerializableService, Serializable>();
            Mvx.LazyConstructAndRegisterSingleton<ISearchService, Search>();
            Mvx.LazyConstructAndRegisterSingleton<ICheckTrainService, CheckTrain>();
            Mvx.LazyConstructAndRegisterSingleton<IFavoriteManageService, FavoriteManage>();
            //Mvx.LazyConstructAndRegisterSingleton<ISearchService, Search>();
            //Mvx.LazyConstructAndRegisterSingleton<ISearchService, Search>();
            //Mvx.LazyConstructAndRegisterSingleton<ISearchService, Search>();
            //Mvx.LazyConstructAndRegisterSingleton<ISearchService, Search>();

            base.InitializePlatformServices();
        }
    }
}