using Cirrious.CrossCore;
using Cirrious.CrossCore.IoC;
using Cirrious.MvvmCross.ViewModels;
using Trains.Core.Interfaces;
using Trains.Core.Services;
using Trains.Core.Services.Interfaces;
using Trains.Core.ViewModels;
using Trains.Model.Entities;

namespace Trains.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();


            RegisterAppStart<SettingsViewModel>();

            Mvx.LazyConstructAndRegisterSingleton<IAppSettings, AppSettings>();

            Mvx.LazyConstructAndRegisterSingleton<IHttpService, BaseHttpService>();
            Mvx.LazyConstructAndRegisterSingleton<ISearchService, SearchService>();
            Mvx.LazyConstructAndRegisterSingleton<ILocalDataService, LocalData>();
            Mvx.LazyConstructAndRegisterSingleton<ITrainStopService, TrainStopService>();
            Mvx.LazyConstructAndRegisterSingleton<IFavoriteManageService, FavoriteManage>();
            Mvx.LazyConstructAndRegisterSingleton<ISerializableService, SerializeService>();

            var lang = Mvx.Resolve<ISerializableService>().Desserialize<Language>(Constants.CurrentLanguage);
            if (lang == null)
                RegisterAppStart<SettingsViewModel>();
            else
            {
                Mvx.Resolve<IAppSettings>().Language = lang;
                RegisterAppStart<MainViewModel>();
            }
        }
    }
}