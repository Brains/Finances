using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace UI.Views.Converters
{
	public class AmountTextToDecimal : IValueConverter
	{
		private readonly IAdder adder;

		public AmountTextToDecimal(IAdder adder)
		{
			this.adder = adder;
		}

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

			return adder.Convert(amount);
		}
	}
}