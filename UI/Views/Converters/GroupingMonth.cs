using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace UI.Views.Converters
{
	internal class GroupingMonth : IValueConverter
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