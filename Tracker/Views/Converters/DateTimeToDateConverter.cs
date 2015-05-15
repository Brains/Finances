using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Tracker.Views.Converters
{
	public class DateTimeToDateConverter : IValueConverter
	{
		public object Convert (object value, Type targetType, object parameter, CultureInfo culture)
		{
			return ((DateTime) value).ToString("D");
		}

		public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
		{
			return DependencyProperty.UnsetValue;
		}
	}
}
