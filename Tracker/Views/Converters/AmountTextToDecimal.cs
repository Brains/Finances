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
		//------------------------------------------------------------------
		public object Convert (object value, Type targetType, object parameter, CultureInfo culture)
		{
			decimal amount = (decimal) value;

			return amount.ToString(CultureInfo.InvariantCulture);
		}

		//------------------------------------------------------------------
		public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
		{
			string amount = (string) value;
			string[] amounts = amount.Split('+');
			decimal[] decimals = amounts.Select(decimal.Parse).ToArray();

			return decimals.Sum().ToString(CultureInfo.InvariantCulture);
		}
	}
}