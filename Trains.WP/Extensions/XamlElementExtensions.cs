using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Trains.Core;
using Trains.Infrastructure;

namespace Trains.WP.Extensions
{
	public static class XamlElementExtensions
	{
		public static readonly DependencyProperty ResourceProperty =
			DependencyProperty.RegisterAttached(Defines.I18n.RESOURCE, typeof(string), typeof(XamlElementExtensions), new PropertyMetadata(string.Empty, OnResourceChanged));
		public static string GetResource(DependencyObject obj) { return (string)obj.GetValue(ResourceProperty); }
		public static void SetResource(DependencyObject obj, string value) { obj.SetValue(ResourceProperty, value); }

		private static void OnResourceChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
		{
			ApplyResource(dependencyObject, Dependencies.LocalizationService.GetString(e.NewValue.ToString()));
		}

		private static void ApplyResource(DependencyObject dependencyObject, string str)
		{
			if (dependencyObject is TextBlock)
				((TextBlock)dependencyObject).Text = str;
			else if (dependencyObject is AppBarButton)
				((AppBarButton)dependencyObject).Label = str;
			else if (dependencyObject is Run)
				((Run)dependencyObject).Text = str;
			else if (dependencyObject is Button)
				((Button)dependencyObject).Content = str;
			else if (dependencyObject is TextBox)
				((TextBox)dependencyObject).PlaceholderText = str;
			else if (dependencyObject is ComboBox)
				((ComboBox)dependencyObject).Header = str;
			else if (dependencyObject is PasswordBox)
				((PasswordBox)dependencyObject).PlaceholderText = str;
			else if (dependencyObject is PivotItem)
				((PivotItem)dependencyObject).Header = str;
			else if (dependencyObject is AutoSuggestBox)
				((AutoSuggestBox)dependencyObject).Header = str;
			else if (dependencyObject is ListView)
				((ListView)dependencyObject).Header = str;
			else if (dependencyObject is TimePicker)
				((TimePicker)dependencyObject).Header = str;

		}
	}
}
