using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Finances.Converters
{
	internal class NumberToMonthConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return ((DateTime)value).ToString("MMMM");
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return DependencyProperty.UnsetValue;
		}
	}
}