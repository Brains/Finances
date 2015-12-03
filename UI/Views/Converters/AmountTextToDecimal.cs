using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace UI.Views.Converters
{
	public class AmountTextToDecimal : IValueConverter
	{
		public object Convert (object value, Type targetType, object parameter, CultureInfo culture)
		{
			decimal amount = (decimal) value;

			if (amount == 0)
				return string.Empty;

			return amount.ToString(CultureInfo.InvariantCulture);
		}

		public object ConvertBack (object value, Type targetType, object parameter, CultureInfo culture)
		{
			string amount = (string) value;

			return Summarize(amount);
		}

		public decimal Summarize(string amount)
		{
			if (string.IsNullOrEmpty(amount))
				throw new ArgumentNullException("Empty");

			string[] amounts = amount.Split('+');
			decimal[] decimals = amounts.Select(decimal.Parse).ToArray();
			var sum = decimals.Sum();

			if (sum <= 0)
				throw new ArgumentException("Negative");

			return sum;
		}
	}
}