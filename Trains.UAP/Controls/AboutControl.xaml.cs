using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Trains.Core.ViewModels;
using Trains.Model.Entities;
using Trains.UAP.Views;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Trains.UAP.Controls
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AboutControl : Page
    {
        public AboutControl()
        {
            this.InitializeComponent();
        }

        Dictionary<AboutPicture, Type> AboutItemsActions = new Dictionary<AboutPicture, Type>
            {
            {AboutPicture.AboutApp,typeof(AboutView)},
            {AboutPicture.Settings,typeof(SettingsView)},
            {AboutPicture.Share,typeof(ShareView)}
            };

        private void ListViewBase_OnItemClick(object sender, ItemClickEventArgs e)
        {
            var item = AboutItemsActions[((About) e.ClickedItem).Item];
            if (item != null) Frame.Navigate(item);
            else
                ((MainViewModel) DataContext).TappedAboutItemCommand.Execute(e.ClickedItem);
        }
    }
}
