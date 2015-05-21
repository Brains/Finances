using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace Tracker.Views.Converters
{
	public class AmountTextToDecimal : IValueConverter
	{
		public object Convert (object value, Type targetType, object parameter, CultureInfo culture)
		{
			decimal amount = (decimal) value;

			if (amount == 0)
				return DependencyProperty.UnsetValue;

			return amount.ToString(CultureInfo.InvariantCulture);
		}

		public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
		{
			string amount = (string) value;

			if (string.IsNullOrEmpty(amount))
				return DependencyProperty.UnsetValue;

			return Summarize(amount);
		}

		private static decimal Summarize(string amount)
		{
			string[] amounts = amount.Split('+');
			decimal[] decimals = amounts.Select(decimal.Parse).ToArray();
			var sum = decimals.Sum();

			return sum;
		}
	}
}