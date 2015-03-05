﻿using System.Collections.Generic;
using Caliburn.Micro;
using TrainSearch.Entities;
using TrainSearch.Infrastructure;

namespace TrainShedule_HubVersion.ViewModels
{
    public class HelpPageViewModel : Screen
    {
        private static IEnumerable<MenuDataItem> _menu;
        public IEnumerable<MenuDataItem> Menu
        {
            get { return _menu; }
            set
            {
                _menu = value;
                NotifyOfPropertyChange(() => Menu);
            }
        }

        protected override async void OnActivate()
        {
            Menu = await MenuData.GetItemsAsync();
        }
    }
}