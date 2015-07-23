using Android.App;
using Android.OS;
using Android.Widget;
using Cirrious.MvvmCross.Droid.Views;
using Trains.Core.ViewModels;
using Trains.Model.Entities;
using System.Linq;

namespace Trains.Droid.Views
{
	[Activity(Label = "SettingsView")]
	public class SettingsView : MvxActivity
	{
		private SettingsViewModel Model
		{
			get{ return (SettingsViewModel)ViewModel;}
		}

		Button _resetSettingsButton;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.SettingsView);

			Spinner spinner = FindViewById<Spinner> (Resource.Id.spinner);

			_resetSettingsButton = FindViewById<Button> (Resource.Id.resetbutton);
			_resetSettingsButton.Click += (sender, e) => {SentMessage ();};
			var adapter = ArrayAdapter.CreateFromResource (
				this, Resource.Array.planets_array, Android.Resource.Layout.SimpleSpinnerItem);

			adapter.SetDropDownViewResource (Android.Resource.Layout.SimpleSpinnerDropDownItem);
			spinner.Adapter = adapter;
			spinner.ItemSelected += (spinner_ItemSelected);
			spinner.SetSelection(Model.Languages.FindIndex (x=>x==Model.SelectedLanguage));
		}

		private void spinner_ItemSelected (object sender, AdapterView.ItemSelectedEventArgs e)
		{
			var lang = Model.Languages.First (x => x.Name == ((string)((Spinner)sender).SelectedItem));
			Model.SelectedLanguage = lang;
			if (!string.IsNullOrEmpty (Model.NeedReboot))
				SentMessage();
		}

		private void SentMessage()
		{
			Toast.MakeText (this, Model.NeedReboot, ToastLength.Long).Show ();
		}

		public override void OnAttachedToWindow()
		{
			base.OnAttachedToWindow();
			Window.SetTitle(Model.Header);
		}
	}
}