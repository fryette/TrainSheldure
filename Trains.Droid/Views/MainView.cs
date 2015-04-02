using Android.App;
using Android.OS;
using Cirrious.MvvmCross.Droid.Views;

namespace Trains.Droid.Views
{
    [Activity(Label = "MainView")]
    public class MainView : MvxActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.MainView);
        }
    }
}