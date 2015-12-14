using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace UI.Views.Converters
{
	public class EnumToIcon : IValueConverter
	{
		public object Convert (object value, Type targetType, object parameter, CultureInfo culture)
		{
			string name = value?.ToString();

			if (string.IsNullOrEmpty(name))
				return null;

			var visual = Application.Current.FindResource(name);
			return visual as Visual;
		}

		public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
		{
			return DependencyProperty.UnsetValue;
		}
	}
}