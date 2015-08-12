using Windows.ApplicationModel.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Trains.Core.Resources;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Trains.Universal.Controls
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainControl
	{
		public MainControl()
		{
			this.InitializeComponent();
			SizeChanged += MainControl_SizeChanged;
			Loaded += OnLoaded;
		}

		private void MainControl_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			RootPanel.Width = e.NewSize.Width > 720 ? (e.NewSize.Width-40) * 0.6 : e.NewSize.Width-40;
		}

		private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
		{
			CoreApplication.GetCurrentView().CoreWindow.IsInputEnabled = false;
			CoreApplication.GetCurrentView().CoreWindow.IsInputEnabled = true;
			SetVisibility(Visibility.Visible);
			SetVisibilityAutossugestBox(Visibility.Visible, Visibility.Visible);
			comboBox_SelectionChanged(null, null);
		}

		private void SetVisibility(Visibility visibility)
		{
			comboBox.Visibility = visibility;
			SearchButton.Visibility = visibility;
			Routes.Visibility = visibility;
		}

		void SetVisibilityAutossugestBox(Visibility from, Visibility to)
		{
			From.Visibility = from;
			To.Visibility = to;
		}

		private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			DataPicker.Visibility = comboBox.SelectedItem == ResourceLoader.Instance.Resource["OnDay"] ? Visibility.Visible : Visibility.Collapsed;
		}

		private void OnLoaded(object sender, RoutedEventArgs e)
		{
			To.IsSuggestionListOpen = false;
			To.IsSuggestionListOpen = true;
			From.IsSuggestionListOpen = false;
			From.IsSuggestionListOpen = true;
		}
	}
}
