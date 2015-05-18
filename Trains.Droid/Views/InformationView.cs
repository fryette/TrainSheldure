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
using Cirrious.MvvmCross.Binding.Droid.Views;

namespace Trains.Droid.Views
{
	[Activity(Label = "InformationView")]
	public class InformationView : MvxActivity
	{
		private MvxListView _listView;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			SetContentView(Resource.Layout.InformationView);

			_listView = FindViewById<MvxListView> (Resource.Id.StopPointsListView);
		}

		protected override void OnStart()
		{
			base.OnStart();
		}
	}
}