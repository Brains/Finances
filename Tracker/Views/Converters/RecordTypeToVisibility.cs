using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Tracker.Views.Converters
{
	public class RecordTypeToVisibility : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null) return null;

			Record.Types type = (Record.Types) value;
			string element = (string) parameter;

			bool hide = type == Record.Types.Debt;

			if (element == "ComboBox")
				hide = !hide;

			return hide ? Visibility.Collapsed : Visibility.Visible;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return DependencyProperty.UnsetValue;
		}
	}
}