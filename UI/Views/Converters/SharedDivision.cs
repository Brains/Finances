using System;
using System.Globalization;
using System.Windows.Data;
using Common;
using UI.Interfaces;

namespace UI.Views.Converters
{
	public class SharedDivision : IValueConverter
	{
		private IValueConverter decorated;

		public SharedDivision(IValueConverter decorated)
		{
			this.decorated = decorated;
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return decorated.Convert(value, targetType, parameter, culture);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var amount = (decimal) decorated.ConvertBack(value, targetType, parameter, culture);
			var form = (IForm) parameter;

			if (form.SelectedType == Record.Types.Shared)
			{
				var customers = 3;
				return Math.Round(amount / customers);
			}

			return amount;
		}
	}
}