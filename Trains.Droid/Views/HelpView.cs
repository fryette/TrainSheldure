using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Cirrious.MvvmCross.Droid.Views;
using System;
using System.Collections.Generic;
using Trains.Core.ViewModels;
using Android.Views;
using System.Threading.Tasks;
using Cirrious.MvvmCross.Binding.Droid.Views;
using Cirrious.MvvmCross.ViewModels;

namespace Trains.Droid.Views
{
	[Activity(Label = "View for HelpViewModel")]
	public class HelpView : MvxTabActivity
	{
		private const string DateFormat = "d";

		private Button _searchDateButton;
		private Button _searchTrainButton;
		private Button _searchTypeButton;
		private IMenuItem _updateMenuItem;
		private IMenuItem _swapMenuItem;
		private AutoCompleteTextView _fromTextView;
		private AutoCompleteTextView _toTextView;
		private ProgressBar _progressBar;

		private HelpViewModel Model
		{
			get{ return (HelpViewModel)ViewModel;}
		}

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			RequestWindowFeature (WindowFeatures.NoTitle);
			SetContentView(Resource.Layout.HelpView);

			TabHost.TabSpec spec;

			spec = TabHost.NewTabSpec("train");
			spec.SetIndicator(Model.Trains);
			spec.SetContent(Resource.Id.tab1);
			TabHost.AddTab(spec);

			spec = TabHost.NewTabSpec("carriage");
			spec.SetIndicator(Model.Carriage);
			spec.SetContent(Resource.Id.tab2);
			TabHost.AddTab(spec);

			spec = TabHost.NewTabSpec("place");
			spec.SetIndicator(Model.Other);
			spec.SetContent(Resource.Id.tab3);
			TabHost.AddTab(spec);

			_progressBar = FindViewById<ProgressBar> (Resource.Id.progressBar);
			_searchTrainButton = FindViewById<Button>(Resource.Id.SearchTrain);
			_searchDateButton = FindViewById<Button>(Resource.Id.SearchDate);
			_searchTypeButton = FindViewById<Button>(Resource.Id.SearchType);
			_fromTextView = FindViewById<AutoCompleteTextView>(Resource.Id.FromTextView);
			_toTextView = FindViewById<AutoCompleteTextView>(Resource.Id.ToTextView);

		}
	}
}