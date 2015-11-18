using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Temporary.Converters
{
	public class DateTimeToDateConverter : IValueConverter
	{
		public object Convert (object value, Type targetType, object parameter, CultureInfo culture)
		{
			return ((DateTime) value).ToString("d MMMM, dddd");
		}

		public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
		{
			return DependencyProperty.UnsetValue;
		}
	}
}
