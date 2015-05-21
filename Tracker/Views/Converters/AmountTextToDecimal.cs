using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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

			return Summarize(amount);
		}

		public static decimal Summarize(string amount)
		{
			if (string.IsNullOrEmpty(amount))
				throw new NullReferenceException("Empty");

			string[] amounts = amount.Split('+');
			decimal[] decimals = amounts.Select(decimal.Parse).ToArray();
			var sum = decimals.Sum();

			if (sum <= 0)
				throw new InvalidDataException("Negative");

			return sum;
		}
	}
}