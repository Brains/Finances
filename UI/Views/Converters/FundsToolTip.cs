using System;
using System.Globalization;
using System.Windows;

namespace UI.Views.Converters
{
	public class FundsToolTip : Base
	{
		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			dynamic source = value;

			if (source.Name == "Debts")
				return parameter;

			return DependencyProperty.UnsetValue;
		}
	}
}