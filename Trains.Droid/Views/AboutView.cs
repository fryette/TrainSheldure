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
	[Activity(Label = "AboutView")]
	public class AboutView : MvxActivity
	{
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			SetContentView(Resource.Layout.AboutView);
		}

		public override void OnAttachedToWindow()
		{
			base.OnAttachedToWindow();
			Window.SetTitle(((AboutViewModel)ViewModel).AboutUs);
		}
	}
}