using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Cirrious.MvvmCross.Droid.Views;
using Trains.Core.ViewModels;

namespace Trains.Droid.Views
{
	[Activity(Label = "ScheduleView")]
	public class ScheduleView : MvxActivity
	{
		private IMenuItem _favoriteMenuItem;
		private IMenuItem _unFavoriteMenuItem;

		private ScheduleViewModel Model
		{
			get{ return (ScheduleViewModel)ViewModel;}
		}

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			SetContentView(Resource.Layout.ScheduleView);
		}

		public override bool OnCreateOptionsMenu(IMenu menu)
		{
			MenuInflater.Inflate(Resource.Menu.schedule_menu, menu);
			_favoriteMenuItem = menu.FindItem (Resource.Id.favorite);
			_unFavoriteMenuItem = menu.FindItem(Resource.Id.unFavorite);
			SetAppBarVisibility ();
			return base.OnPrepareOptionsMenu(menu);
		}

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			switch (item.ItemId)
			{
			case Resource.Id.favorite:
				{
					Model.AddToFavoriteCommand.Execute ();
					SetAppBarVisibility ();
					return true;
				}
			case Resource.Id.unFavorite:
				{
					Model.DeleteInFavoriteCommand.Execute ();
					SetAppBarVisibility ();
					return true;
				}
			case Resource.Id.help:
				{
					Model.GoToHelpPageCommand.Execute ();
					return true;
				}
			case Resource.Id.reverseSearch:
				{
					Model.SearchReverseRouteCommand.Execute ();
					return true;
				}
			}
			return base.OnOptionsItemSelected(item);
		}

		private void SetAppBarVisibility()
		{
			_favoriteMenuItem.SetVisible(Model.IsVisibleFavoriteIcon);
			_unFavoriteMenuItem.SetVisible(!Model.IsVisibleFavoriteIcon);
		}

		public override void OnBackPressed()
		{
			var intent =	new Intent (this, typeof(MainView));
			intent.SetFlags (ActivityFlags.ClearTop);
			StartActivity (intent);
		}
	}
}