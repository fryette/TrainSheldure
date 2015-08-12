using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace Trains.UAP.Triggers
{
	public class IsFullScreenModeTrigger : StateTriggerBase
	{
		public IsFullScreenModeTrigger()
		{
			ApplicationView view = ApplicationView.GetForCurrentView();

			SetActive(view.IsFullScreenMode);

			Window.Current.SizeChanged += CurrentWindow_SizeChanged;
		}

		private void CurrentWindow_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
		{
			ApplicationView view = ApplicationView.GetForCurrentView();

			SetActive(view.IsFullScreen);
		}
	}
}