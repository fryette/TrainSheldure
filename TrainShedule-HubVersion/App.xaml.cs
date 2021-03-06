﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Globalization;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Caliburn.Micro;
using Trains.App.ViewModels;
using Trains.App.Views;
using Language = Trains.Model.Entities.Language;
using TrainStop = Trains.Services.Implementations.TrainStop;

namespace Trains.App
{
    public sealed partial class App
    {
        public static Func<string, Task> ShowMessageDialog { get; set; }
        public App()
        {
            InitializeComponent();
            SetLanguage();
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }

        private WinRTContainer _container;

        protected override void Configure()
        {
            _container = new WinRTContainer();

            _container.RegisterWinRTServices();

            MessageBinder.SpecialValues.Add("$clickeditem", c => ((ItemClickEventArgs)c.EventArgs).ClickedItem);
            MessageBinder.SpecialValues.Add("$clickeditem1", c => ((PivotItemEventArgs)c.EventArgs).Item);

            _container.Singleton<ILastRequestTrainService, LastRequestTrain>();
            _container.Singleton<ISearchService, Search>();
            _container.Singleton<ISerializableService, Serializable>();
            _container.Singleton<ITrainStopService, TrainStop>();
            _container.Singleton<ICheckTrainService, CheckTrain>();
            _container.Singleton<IStartService, Start>();
            _container.Singleton<IFavoriteManageService, FavoriteManage>();

            _container.PerRequest<SettingsViewModel>();
            _container.PerRequest<AboutViewModel>();
            _container.PerRequest<HelpViewModel>();
            _container.PerRequest<EditFavoriteRoutesViewModel>();
            _container.PerRequest<StopPointViewModel>();
            _container.PerRequest<ScheduleViewModel>();
            _container.PerRequest<InformationViewModel>();
            _container.PerRequest<ItemViewModel>();
            _container.PerRequest<MainViewModel>();
        }

        protected override void PrepareViewFirst(Frame rootFrame)
        {
            _container.RegisterNavigationService(rootFrame);
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            DisplayRootView<MainView>();
        }

        protected override object GetInstance(Type service, string key)
        {
            return _container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }

        private static void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            var frame = Window.Current.Content as Frame;
            if (frame == null) return;
            if (!frame.CanGoBack) return;
            frame.GoBack();
            e.Handled = true;
        }

        private static async void SetLanguage()
        {
            SavedItems.Lang = await Serialize.ReadObjectFromXmlFileAsync<Language>(FileName.CurrentLanguage);
            ApplicationLanguages.PrimaryLanguageOverride = SavedItems.Lang == null ? "ru" : SavedItems.Lang.Id;
        }
    }
}
