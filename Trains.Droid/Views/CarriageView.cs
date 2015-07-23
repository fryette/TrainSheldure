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
using Trains.Model.Entities;
using Trains.Core.ViewModels;

namespace Trains.Droid.Views
{
	[Activity(Label = "CarriageView")]
    public class CarriageView : MvxActivity
	{
			CarriageViewModel Model
			{
				get{ return (CarriageViewModel)ViewModel;}
			}

		readonly Dictionary<Carriage, int> CarriagePictures=new Dictionary<Carriage, int>()
			{
			{Carriage.FirstClassSleeper,Resource.Drawable.FirstClassSleeper},
			{Carriage.CompartmentSleeper,Resource.Drawable.CompartmentSleeper},
			{Carriage.EconomyClassSleeper,Resource.Drawable.EconomyClassSleeper},
			{Carriage.SeatingCoaches1,Resource.Drawable.SeatingCoaches1},
			{Carriage.SeatingCoaches2,Resource.Drawable.SeatingCoaches2},
			{Carriage.MultipleUnitCars1,Resource.Drawable.MultipleUnitCars1},
			{Carriage.MultipleUnitCars2,Resource.Drawable.MultipleUnitCars2},
			{Carriage.MultipleUnitCoach,Resource.Drawable.MultipleUnitCoach}
			};

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			RequestWindowFeature (WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.CarriageView);
		}

		protected override void OnStart()
		{
			FindViewById<ImageView>(Resource.Id.imageview)
				.SetImageResource(CarriagePictures[((CarriageViewModel)ViewModel).CarriageModel.Carriage]);
			base.OnStart();
		}
	}
}